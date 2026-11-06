using Domain.Entities;
using Microsoft.Extensions.Logging;
using Services.Listings;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.AdPayments;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.AdPayments;

public class AdPaymentService : IAdPaymentService
{
    public const string ScreenshotFolder = "ad-payments";

    public const int MaxInstructionsLength = 1000;

    public const int MaxTransactionReferenceLength = 100;

    public const decimal MaxPrice = 1_000_000m;

    private readonly IAdPaymentRepository _repository;
    private readonly IAdminAdRepository _listings;
    private readonly IAdminAdService _adminAds;
    private readonly IPrivateFileStore _files;
    private readonly INotificationService _notifications;
    private readonly IAdminAlertService _alerts;
    private readonly IAdminAuditService _audit;
    private readonly ILookupService _lookups;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdPaymentService> _logger;

    public AdPaymentService(
        IAdPaymentRepository repository,
        IAdminAdRepository listings,
        IAdminAdService adminAds,
        IPrivateFileStore files,
        INotificationService notifications,
        IAdminAlertService alerts,
        IAdminAuditService audit,
        ILookupService lookups,
        IUnitOfWork unitOfWork,
        ILogger<AdPaymentService> logger)
    {
        _repository = repository;
        _listings = listings;
        _adminAds = adminAds;
        _files = files;
        _notifications = notifications;
        _alerts = alerts;
        _audit = audit;
        _lookups = lookups;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public static string ScreenshotRoute(Guid paymentId) =>
        $"/{ApiVersions.AdminRoutePrefix}/ad-payment/submissions/{paymentId}/screenshot";

    public async Task<PublicAdPaymentConfigDto> GetPublicConfigAsync(CancellationToken cancellationToken = default)
    {
        var settings = await LoadSettingsAsync(cancellationToken);
        var methods = await OfferedMethodsAsync(settings, cancellationToken);

        return new PublicAdPaymentConfigDto
        {
            IsEnabled = settings.IsEnabled,
            Price = settings.Price,
            Currency = settings.Currency,
            Instructions = settings.Instructions,
            PaymentMethods = settings.IsEnabled ? methods.Select(ToDto).ToList() : Array.Empty<AdPaymentMethodDto>(),
            UpdatedAt = settings.UpdatedAt
        };
    }

    public IReadOnlyList<AdPaymentModuleDto> GetModules() =>
        ListingModuleCatalog.All
            .Select(type => new AdPaymentModuleDto
            {
                AdvertisementType = type,
                AdvertisementTypeKey = type.ToString(),
                AdvertisementTypeName = ListingModuleCatalog.NameOf(type),
                CategoryId = (int?)ListingModuleCatalog.CategoryOf(type) ??
                             (type == ListingModuleType.Advertisement
                                 ? (int?)ListingModuleCatalog.CategoryOf(SubCategoryType.Private)
                                 : null),
                SubCategoryId = (int?)ListingModuleCatalog.SubCategoryOf(type),
                ApiRoute = ListingModuleCatalog.RouteOf(type) is { Length: > 0 } route ? "/" + route : null,
                IsSubjectToPayment = AdPaymentCatalog.IsSubjectToPayment(type)
            })
            .ToList();

    public async Task<AdminAdPaymentSettingsDto> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        var settings = await LoadSettingsAsync(cancellationToken);
        var methods = await OfferedMethodsAsync(settings, cancellationToken);
        var ids = ParseIds(settings.PaymentMethodIds);

        string? updatedByName = null;

        if (settings.UpdatedBy is { } updatedBy)
        {
            var names = await _repository.GetUserNamesAsync(new[] { updatedBy }, cancellationToken);
            updatedByName = names.GetValueOrDefault(updatedBy);
        }

        return new AdminAdPaymentSettingsDto
        {
            IsEnabled = settings.IsEnabled,
            Price = settings.Price,
            Currency = settings.Currency,
            Instructions = settings.Instructions,
            PaymentMethods = methods.Select(ToDto).ToList(),
            PaymentMethodIds = ids.Count == 0 ? ids : methods.Select(method => method.Id).ToList(),
            UsesAllActiveMethods = ids.Count == 0,
            EnabledAt = settings.EnabledAt,
            UpdatedAt = settings.UpdatedAt,
            UpdatedBy = settings.UpdatedBy,
            UpdatedByName = updatedByName
        };
    }

    public async Task<AdminAdPaymentSettingsDto> UpdateSettingsAsync(
        UpdateAdPaymentSettingsRequest request, string adminUserId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currency = string.IsNullOrWhiteSpace(request.Currency)
            ? PaymentCatalog.DefaultCurrency
            : request.Currency.Trim().ToUpperInvariant();

        if (currency.Length != 3 || !currency.All(char.IsAsciiLetterUpper))
            throw new BadRequestException("العملة لازم تكون كود من 3 حروف زي EGP.");

        if (request.Price < 0 || request.Price > MaxPrice)
            throw new BadRequestException($"سعر الإعلان لازم يكون بين 0 و {MaxPrice:N0}.");

        if (decimal.Round(request.Price, 2) != request.Price)
            throw new BadRequestException("سعر الإعلان مينفعش يكون فيه أكتر من رقمين عشريين.");

        if (request.IsEnabled && request.Price <= 0)
            throw new BadRequestException("لازم تحدد سعر أكبر من صفر قبل تفعيل الدفع للإعلانات.");

        var instructions = string.IsNullOrWhiteSpace(request.Instructions) ? null : request.Instructions.Trim();

        if (instructions is { Length: > MaxInstructionsLength })
            throw new BadRequestException($"تعليمات الدفع مينفعش تزيد عن {MaxInstructionsLength} حرف.");

        var requestedIds = (request.PaymentMethodIds ?? Array.Empty<int>()).Distinct().ToList();
        var active = await _repository.GetActiveMethodsAsync(cancellationToken);
        var activeIds = active.Select(method => method.Id).ToHashSet();

        var unknown = requestedIds.Where(id => !activeIds.Contains(id)).ToList();

        if (unknown.Count > 0)
            throw new BadRequestException(
                $"طرق الدفع دي مش موجودة أو مش مفعلة: {string.Join("، ", unknown)}.");

        if (request.IsEnabled && active.Count == 0)
            throw new BadRequestException("لازم تضيف طريقة دفع مفعلة واحدة على الأقل قبل تفعيل الدفع للإعلانات.");

        var settings = await _repository.GetSettingsAsync(asNoTracking: false, cancellationToken);

        if (settings is null)
        {
            settings = new AdPaymentSetting { Id = 1 };
            _repository.AddSettings(settings);
        }

        var before = Describe(settings);
        var now = DateTime.UtcNow;

        if (request.IsEnabled && !settings.IsEnabled)
            settings.EnabledAt = now;

        settings.IsEnabled = request.IsEnabled;
        settings.Price = request.Price;
        settings.Currency = currency;
        settings.Instructions = instructions;
        settings.PaymentMethodIds = requestedIds.Count == 0 ? null : string.Join(",", requestedIds.OrderBy(id => id));
        settings.UpdatedAt = now;
        settings.UpdatedBy = adminUserId;

        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdateSettings, AdminAuditCatalog.Targets.PlatformSettings, "ad-payment",
            "تعديل إعدادات الدفع للإعلانات",
            oldValue: before, newValue: Describe(settings),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        return await GetSettingsAsync(cancellationToken);
    }

    public Task<AdPaymentSubmissionResultDto> SubmitAsync(
        ListingModuleType type, Guid listingId, string userId, SubmitAdPaymentRequest request,
        UploadImageModel? screenshot, CancellationToken cancellationToken = default) =>
        SubmitCoreAsync(type, listingId, userId, request, screenshot, resubmission: false, cancellationToken);

    public Task<AdPaymentSubmissionResultDto> ResubmitAsync(
        ListingModuleType type, Guid listingId, string userId, SubmitAdPaymentRequest request,
        UploadImageModel? screenshot, CancellationToken cancellationToken = default) =>
        SubmitCoreAsync(type, listingId, userId, request, screenshot, resubmission: true, cancellationToken);

    private async Task<AdPaymentSubmissionResultDto> SubmitCoreAsync(
        ListingModuleType type, Guid listingId, string userId, SubmitAdPaymentRequest request,
        UploadImageModel? screenshot, bool resubmission, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var settings = await LoadSettingsAsync(cancellationToken);

        if (!settings.IsEnabled)
            throw new BadRequestException("الدفع للإعلانات مش مفعل حالياً، ومش محتاج تدفع حاجة.");

        var listing = await LoadOwnedListingAsync(type, listingId, userId, cancellationToken);

        if (!AdPaymentCatalog.IsSubjectToPayment(type))
            throw new BadRequestException("النوع ده من الإعلانات مش عليه رسوم نشر.");

        var latest = await _repository.GetLatestForListingAsync(type, listingId, cancellationToken);

        if (resubmission && latest is not { Status: PaymentStatus.Rejected })
        {
            throw latest switch
            {
                null => new BadRequestException(
                    "مفيش عملية دفع مرفوضة للإعلان ده عشان تعيد رفعها. استخدم رفع الدفع العادي."),
                { Status: PaymentStatus.Pending } => new ConflictException(
                    "إثبات الدفع اللي رفعته لسه قيد المراجعة، ومينفعش تعيد رفعه دلوقتي."),
                { Status: PaymentStatus.Approved } => new ConflictException(
                    "تم تأكيد الدفع للإعلان ده بالفعل، ومينفعش تعيد رفعه."),
                _ => (AppException)new ConflictException("آخر عملية دفع للإعلان ده مش مرفوضة، ومينفعش تعيد رفعها.")
            };
        }

        if (await _repository.HasPendingAsync(type, listingId, cancellationToken))
            throw new ConflictException("فيه عملية دفع للإعلان ده قيد المراجعة بالفعل. استنى نتيجة المراجعة.");

        if (await _repository.HasCreditAsync(type, listingId, cancellationToken))
            throw new ConflictException("تم تأكيد الدفع للإعلان ده بالفعل، ومش محتاج تدفع تاني.");

        if (!AdPaymentRules.NeedsNewWindow(listing, DateTime.UtcNow))
            throw new BadRequestException(AdPaymentRules.NotNeededReason(listing));

        if (AdPaymentRules.IsGrandfathered(settings.EnabledAt, listing.ModerationStatus, _listings.CreatedAtOf(listing)))
            throw new BadRequestException("الإعلان ده اتضاف قبل تفعيل رسوم النشر، ومش محتاج دفع.");

        if (request.ExpectedAmount is { } expectedAmount && expectedAmount != settings.Price)
            throw new ConflictException(
                $"رسوم نشر الإعلان اتغيرت. الرسوم الحالية {settings.Price:0.##} {settings.Currency}، " +
                "حدّث الصفحة وراجع المبلغ قبل ما تدفع.");

        if (!string.IsNullOrWhiteSpace(request.ExpectedCurrency) &&
            !string.Equals(request.ExpectedCurrency.Trim(), settings.Currency, StringComparison.OrdinalIgnoreCase))
            throw new ConflictException(
                $"عملة رسوم النشر اتغيرت. العملة الحالية {settings.Currency}، حدّث الصفحة قبل ما تدفع.");

        var reference = string.IsNullOrWhiteSpace(request.TransactionReference)
            ? null
            : request.TransactionReference.Trim();

        if (reference is { Length: > MaxTransactionReferenceLength })
            throw new BadRequestException($"رقم العملية مينفعش يزيد عن {MaxTransactionReferenceLength} حرف.");

        var notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();

        if (notes is { Length: > AdPaymentCatalog.MaxNotesLength })
            throw new BadRequestException($"الملاحظات مينفعش تزيد عن {AdPaymentCatalog.MaxNotesLength} حرف.");

        var methods = await OfferedMethodsAsync(settings, cancellationToken);
        var method = methods.FirstOrDefault(candidate => candidate.Id == request.PaymentMethodId)
            ?? throw new BadRequestException("طريقة الدفع اللي اخترتها مش متاحة لدفع الإعلانات.");

        if (screenshot is null)
            throw new BadRequestException("صورة إيصال الدفع مطلوبة.");

        var stored = await _files.SaveImageAsync(screenshot, ScreenshotFolder, cancellationToken);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PaymentMethodId = method.Id,

            PaymentMethodNameSnapshot = method.ArabicName ?? method.Name,
            PaymentInfoSnapshot = PaymentCatalog.ResolvePaymentInfo(
                method.Type, method.PhoneNumber, method.InstaPayIdentifier, method.AccountNumber),
            AccountNameSnapshot = method.AccountHolderName,
            InstructionsSnapshot = method.Instructions ?? settings.Instructions,

            Amount = settings.Price,
            Currency = settings.Currency,
            ScreenshotPath = stored.RelativePath,
            TransactionReference = reference,
            Notes = notes,

            Status = PaymentStatus.Pending,
            SubmittedAt = DateTime.UtcNow,

            Purpose = PaymentPurpose.AdvertisementPublication,
            TargetType = type.ToString(),
            TargetId = listingId,
            Quantity = 1
        };

        payment.ScreenshotUrl = ScreenshotRoute(payment.Id);

        try
        {
            await _repository.AddAsync(payment, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _files.Delete(stored.RelativePath);

            if (_unitOfWork.IsUniqueConstraintViolation(exception))
                throw new ConflictException("فيه عملية دفع للإعلان ده قيد المراجعة بالفعل. استنى نتيجة المراجعة.");

            throw;
        }

        var isResubmission = latest is { Status: PaymentStatus.Rejected };

        await RunSideEffectAsync(
            async () =>
            {
                var payer = (await _repository.GetUserNamesAsync(new[] { userId }, cancellationToken))
                    .GetValueOrDefault(userId);

                await _alerts.NotifyNewAdPaymentAsync(
                    payment.Id, payment.Amount, payment.Currency, payer, listing.ListingTitle, type,
                    isResubmission, cancellationToken);
            },
            "admin alert", payment.Id);

        await RunSideEffectAsync(
            () => _notifications.CreateAsync(
                userId,
                "استلام الدفع",
                $"💳 استلمنا إثبات دفع رسوم إعلان «{listing.ListingTitle}» بقيمة {payment.Amount:N0} {payment.Currency}، وهو دلوقتي قيد المراجعة.",
                NotificationType.PaymentSubmitted,
                payment.Id,
                NotificationReferenceTypes.Payment,
                action: NotificationAction.Created,
                icon: "💳",
                entityName: NotificationCatalog.Payment.EntityName,
                deepLink: $"{NotificationCatalog.Payment.Route}/{payment.Id}",
                listingType: type,
                categoryId: (int?)ListingModuleCatalog.CategoryOf(type),
                subCategoryId: (int?)ListingModuleCatalog.SubCategoryOf(type)),
            "owner notification", payment.Id);

        return new AdPaymentSubmissionResultDto
        {
            PaymentSubmissionId = payment.Id,
            AdvertisementId = listingId,
            AdvertisementType = type,
            Amount = payment.Amount,
            Currency = payment.Currency,
            PaymentStatus = payment.Status,
            PaymentStatusName = PaymentCatalog.GetStatusName(payment.Status),
            SubmittedAt = payment.SubmittedAt,
            PaymentMethodId = payment.PaymentMethodId,
            PaymentMethodName = payment.PaymentMethodNameSnapshot,
            TransactionReference = payment.TransactionReference,
            Notes = payment.Notes,
            IsResubmission = latest is { Status: PaymentStatus.Rejected },
            PreviousPaymentSubmissionId = latest is { Status: PaymentStatus.Rejected } ? latest.Id : null,
            Stage = AdPaymentCatalog.Stages.PaymentUnderReview,
            StageName = AdPaymentCatalog.Stages.NameOf(AdPaymentCatalog.Stages.PaymentUnderReview),
            StatusEndpoint = AdPaymentCatalog.SubmitRouteOf(type, listingId)
        };
    }

    public async Task<AdPaymentStatusDto> GetStatusAsync(
        ListingModuleType type, Guid listingId, string userId, CancellationToken cancellationToken = default)
    {
        var listing = await LoadOwnedListingAsync(type, listingId, userId, cancellationToken);
        var settings = await LoadSettingsAsync(cancellationToken);
        var latest = await _repository.GetLatestForListingAsync(type, listingId, cancellationToken);

        var needsWindow = AdPaymentRules.NeedsNewWindow(listing, DateTime.UtcNow);
        var hasPending = latest is { Status: PaymentStatus.Pending };
        var hasCredit = await _repository.HasCreditAsync(type, listingId, cancellationToken);

        var grandfathered = AdPaymentRules.IsGrandfathered(
            settings.EnabledAt, listing.ModerationStatus, _listings.CreatedAtOf(listing));

        var required = settings.IsEnabled && AdPaymentCatalog.IsSubjectToPayment(type) &&
                       needsWindow && !grandfathered && !hasCredit;
        var stage = AdPaymentCatalog.ResolveStage(required, latest?.Status, hasCredit);

        return new AdPaymentStatusDto
        {
            AdvertisementId = listingId,
            AdvertisementType = type,
            PaymentsEnabled = settings.IsEnabled,
            IsPaymentRequired = required,
            CanSubmit = required && !hasPending,
            HasPayment = latest is not null,
            PaymentSubmissionId = latest?.Id,
            PaymentStatus = latest?.Status,
            PaymentStatusName = latest is null ? null : PaymentCatalog.GetStatusName(latest.Status),
            Amount = latest?.Amount,
            Currency = latest?.Currency,
            PaymentMethodId = latest?.PaymentMethodId,
            PaymentMethodName = latest?.PaymentMethodNameSnapshot ?? latest?.PaymentMethod?.Name,
            TransactionReference = latest?.TransactionReference,
            SubmittedAt = latest?.SubmittedAt,
            ReviewedAt = latest is null ? null : latest.ApprovedAt ?? latest.RejectedAt,
            RejectionReason = latest?.Status == PaymentStatus.Rejected ? latest.RejectReason : null,
            ReviewFeedback = latest?.Status is PaymentStatus.Approved or PaymentStatus.Rejected
                ? latest.ReviewFeedback
                : null,
            CurrentPrice = settings.IsEnabled ? settings.Price : null,
            CurrentCurrency = settings.IsEnabled ? settings.Currency : null,
            CanResubmit = required && !hasPending && latest is { Status: PaymentStatus.Rejected },
            Stage = stage,
            StageName = AdPaymentCatalog.Stages.NameOf(stage),
            AdvertisementTitle = listing.ListingTitle,
            AdvertisementModerationStatus = listing.ModerationStatus.ToString(),
            AdvertisementModerationStatusName = ModerationCatalog.NameOf(listing.ModerationStatus),
            Notes = latest?.Notes,
            SubmitEndpoint = AdPaymentCatalog.SubmitRouteOf(type, listingId),
            ResubmitEndpoint = AdPaymentCatalog.ResubmitRouteOf(type, listingId)
        };
    }

    public async Task<PaginatedResult<AdminAdPaymentSubmissionDto>> GetSubmissionsAsync(
        AdminAdPaymentFilterParams filter, CancellationToken cancellationToken = default)
    {
        if (filter.FromDate is { } from && filter.ToDate is { } to && from > to)
            throw new BadRequestException("تاريخ البداية لازم يكون قبل تاريخ النهاية.");

        var (items, totalCount) = await _repository.GetPagedAsync(filter, cancellationToken);

        var rows = await MapAsync(items, cancellationToken);

        return new PaginatedResult<AdminAdPaymentSubmissionDto>(rows, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<AdminAdPaymentSubmissionDto> GetSubmissionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await LoadAdPaymentAsync(id, cancellationToken);

        return (await MapAsync(new[] { payment }, cancellationToken))[0];
    }

    public async Task<AdPaymentReviewResultDto> ReviewAsync(
        Guid id, ReviewAdPaymentRequest request, string adminUserId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var decision = AdPaymentDecisions.Parse(request.Decision)
            ?? throw new BadRequestException("القرار لازم يكون موافقة أو رفض.");

        var reason = decision == PaymentStatus.Rejected && !string.IsNullOrWhiteSpace(request.RejectionReason)
            ? request.RejectionReason.Trim()
            : null;

        if (reason is { Length: > PaymentCatalog.MaxRejectReasonLength })
            throw new BadRequestException($"سبب الرفض مينفعش يزيد عن {PaymentCatalog.MaxRejectReasonLength} حرف.");

        var feedback = string.IsNullOrWhiteSpace(request.Feedback) ? null : request.Feedback.Trim();

        if (feedback is { Length: > PaymentCatalog.MaxReviewFeedbackLength })
            throw new BadRequestException(
                $"ملاحظات المراجعة مينفعش تزيد عن {PaymentCatalog.MaxReviewFeedbackLength} حرف.");

        var payment = await LoadAdPaymentAsync(id, cancellationToken);

        if (string.Equals(payment.UserId, adminUserId, StringComparison.Ordinal))
            throw new ForbiddenException("مينفعش تراجع عملية دفع إنت اللي دافعها.");

        if (payment.Status != PaymentStatus.Pending)
            throw new ConflictException(
                $"تم البت في عملية الدفع بالفعل ({PaymentCatalog.GetStatusName(payment.Status)}).");

        var now = DateTime.UtcNow;

        var decided = await _repository.DecideAsync(
            id, decision, adminUserId, reason, feedback, now, cancellationToken);

        if (decided == 0)
            throw new ConflictException("تم البت في عملية الدفع دي من مسؤول تاني في نفس اللحظة.");

        var type = TypeOf(payment);
        var listingId = payment.TargetId!.Value;

        await _audit.LogAsync(
            decision == PaymentStatus.Approved ? AdminAuditAction.ApprovePayment : AdminAuditAction.RejectPayment,
            AdminAuditCatalog.Targets.Payment, id.ToString(),
            decision == PaymentStatus.Approved
                ? $"تأكيد دفع رسوم إعلان بقيمة {payment.Amount:N0} {payment.Currency}"
                : reason is null
                    ? $"رفض دفع رسوم إعلان بقيمة {payment.Amount:N0} {payment.Currency}"
                    : $"رفض دفع رسوم إعلان بقيمة {payment.Amount:N0} {payment.Currency} — {reason}",
            oldValue: PaymentCatalog.GetStatusName(PaymentStatus.Pending),
            newValue: PaymentCatalog.GetStatusName(decision),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        var action = AdPaymentListingActions.None;
        string? moderation = null;

        if (decision == PaymentStatus.Approved)
            (action, moderation) = await ContinueListingAsync(type, listingId, adminUserId, cancellationToken);

        var title = (await _repository.GetListingSummariesAsync(new[] { (type, listingId) }, cancellationToken))
            .GetValueOrDefault((type, listingId))?.Title;

        await RunSideEffectAsync(
            () => _notifications.CreateAsync(
                payment.UserId,
                decision == PaymentStatus.Approved ? "تأكيد الدفع" : "رفض الدفع",
                decision == PaymentStatus.Approved
                    ? ApprovedMessage(title, action) + FeedbackSuffix(feedback)
                    : RejectedMessage(title, reason, feedback),
                decision == PaymentStatus.Approved ? NotificationType.PaymentApproved : NotificationType.PaymentRejected,
                payment.Id,
                NotificationReferenceTypes.Payment,
                action: NotificationAction.Updated,
                icon: decision == PaymentStatus.Approved ? "✅" : "❌",
                entityName: NotificationCatalog.Payment.EntityName,
                deepLink: $"{NotificationCatalog.Payment.Route}/{payment.Id}",
                listingType: type,
                categoryId: (int?)ListingModuleCatalog.CategoryOf(type),
                subCategoryId: (int?)ListingModuleCatalog.SubCategoryOf(type)),
            "owner notification", payment.Id);

        return new AdPaymentReviewResultDto
        {
            Payment = await GetSubmissionAsync(id, cancellationToken),
            ListingAction = action,
            ListingActionName = ActionName(action),
            ListingModerationStatus = moderation
        };
    }

    public async Task<PrivateFileContent> OpenScreenshotAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await LoadAdPaymentAsync(id, cancellationToken);

        return _files.Open(payment.ScreenshotPath)
               ?? throw new NotFoundException("صورة إيصال الدفع مش موجودة.");
    }

    public async Task<bool> IsAdPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default) =>
        await _repository.GetByIdAsync(paymentId, cancellationToken) is { Purpose: PaymentPurpose.AdvertisementPublication };

    public async Task EnsureMethodCanBeWithdrawnAsync(int paymentMethodId, CancellationToken cancellationToken = default)
    {
        var settings = await LoadSettingsAsync(cancellationToken);

        if (!settings.IsEnabled)
            return;

        var offered = await OfferedMethodsAsync(settings, cancellationToken);

        if (offered.Count == 1 && offered[0].Id == paymentMethodId)
            throw new ConflictException(
                "طريقة الدفع دي هي الوحيدة المتاحة لدفع رسوم الإعلانات، ولو اتوقفت مش هيبقى فيه طريقة يدفع بيها أصحاب الإعلانات " +
                "ومش هينفع تتم الموافقة على أي إعلان جديد. ضيف طريقة دفع تانية لدفع الإعلانات أو وقف الدفع للإعلانات الأول.");
    }

    private async Task<(string Action, string? Moderation)> ContinueListingAsync(
        ListingModuleType type, Guid listingId, string adminUserId, CancellationToken cancellationToken)
    {
        var listing = await _listings.FindForModerationAsync(type, listingId, cancellationToken);

        if (listing is null)
            return (AdPaymentListingActions.CreditKept, null);

        if (listing.ModerationStatus != ModerationStatus.Approved)
            return (
                listing.ModerationStatus == ModerationStatus.Pending
                    ? AdPaymentListingActions.AwaitingModeration
                    : AdPaymentListingActions.CreditKept,
                listing.ModerationStatus.ToString());

        if (!AdPaymentRules.NeedsNewWindow(listing, DateTime.UtcNow))
            return (AdPaymentListingActions.CreditKept, listing.ModerationStatus.ToString());

        try
        {
            var result = await _adminAds.RepublishAsync(type, listingId, adminUserId, cancellationToken);

            return (AdPaymentListingActions.Republished, result.Moderation.Status);
        }
        catch (AppException exception)
        {
            _logger.LogWarning(exception,
                "Payment for {Module} {ListingId} was approved but the republish could not run; the credit is kept.",
                type, listingId);

            return (AdPaymentListingActions.CreditKept, listing.ModerationStatus.ToString());
        }
    }

    private async Task<IModeratedListing> LoadOwnedListingAsync(
        ListingModuleType type, Guid listingId, string userId, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(type))
            throw new NotFoundException("الإعلان غير موجود.");

        var listing = await _listings.FindForModerationAsync(type, listingId, cancellationToken);

        if (listing is null ||
            !string.Equals(listing.OwnerUserId, userId, StringComparison.Ordinal) ||
            AdPaymentRules.IsDeleted(listing) ||
            (listing is LostFoundPost post && ListingModuleCatalog.ModuleOf(post.PostType) != type))
        {
            throw new NotFoundException("الإعلان غير موجود.");
        }

        return listing;
    }

    private async Task<Payment> LoadAdPaymentAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(id, cancellationToken);

        if (payment is not { Purpose: PaymentPurpose.AdvertisementPublication, TargetId: not null } ||
            !Enum.TryParse<ListingModuleType>(payment.TargetType, out _))
        {
            throw new NotFoundException("عملية الدفع غير موجودة.");
        }

        return payment;
    }

    private async Task<AdPaymentSetting> LoadSettingsAsync(CancellationToken cancellationToken) =>
        await _repository.GetSettingsAsync(asNoTracking: true, cancellationToken)
        ?? new AdPaymentSetting { Id = 1, IsEnabled = false, Price = 0, Currency = PaymentCatalog.DefaultCurrency };

    private async Task<IReadOnlyList<PaymentMethod>> OfferedMethodsAsync(
        AdPaymentSetting settings, CancellationToken cancellationToken)
    {
        var active = await _repository.GetActiveMethodsAsync(cancellationToken);
        var ids = ParseIds(settings.PaymentMethodIds);

        return ids.Count == 0 ? active : active.Where(method => ids.Contains(method.Id)).ToList();
    }

    private async Task<IReadOnlyList<AdminAdPaymentSubmissionDto>> MapAsync(
        IReadOnlyList<Payment> payments, CancellationToken cancellationToken)
    {
        if (payments.Count == 0)
            return Array.Empty<AdminAdPaymentSubmissionDto>();

        var keys = payments.Select(payment => (TypeOf(payment), payment.TargetId!.Value)).Distinct().ToList();
        var summaries = await _repository.GetListingSummariesAsync(keys, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        return payments.Select(payment =>
        {
            var type = TypeOf(payment);
            var listingId = payment.TargetId!.Value;
            var summary = summaries.GetValueOrDefault((type, listingId));

            var subCategoryId = summary?.SubCategoryId ?? (int?)ListingModuleCatalog.SubCategoryOf(type);
            var categoryId = summary?.CategoryId ?? (int?)ListingModuleCatalog.CategoryOf(type);

            var (categoryName, subCategoryName) =
                ListingInteractionService.ResolveNames(tree, categoryId ?? 0, subCategoryId ?? 0);

            return new AdminAdPaymentSubmissionDto
            {
                PaymentSubmissionId = payment.Id,
                AdvertisementId = listingId,
                AdvertisementType = type,
                AdvertisementTypeName = ListingModuleCatalog.NameOf(type),
                AdvertisementTitle = summary?.Title,
                CategoryId = categoryId,
                CategoryName = string.IsNullOrEmpty(categoryName) ? null : categoryName,
                SubCategoryId = subCategoryId,
                SubCategoryName = string.IsNullOrEmpty(subCategoryName) ? null : subCategoryName,
                OwnerId = payment.UserId,
                OwnerName = payment.User is null
                    ? null
                    : $"{payment.User.FirstName} {payment.User.SecondName}".Trim(),
                OwnerPhone = payment.User?.PhoneNumber,
                OwnerEmail = payment.User?.Email,
                Amount = payment.Amount,
                Currency = payment.Currency,
                PaymentMethodId = payment.PaymentMethodId,
                PaymentMethodName = payment.PaymentMethodNameSnapshot ?? payment.PaymentMethod?.Name ?? string.Empty,
                PaymentInfo = payment.PaymentInfoSnapshot,
                TransactionReference = payment.TransactionReference,
                SubmittedAt = payment.SubmittedAt,
                PaymentStatus = payment.Status,
                PaymentStatusName = PaymentCatalog.GetStatusName(payment.Status),
                ReviewedAt = payment.ApprovedAt ?? payment.RejectedAt,
                ReviewedBy = payment.ApprovedBy,
                ReviewedByName = payment.Reviewer is null
                    ? null
                    : $"{payment.Reviewer.FirstName} {payment.Reviewer.SecondName}".Trim(),
                RejectionReason = payment.RejectReason,
                ReviewFeedback = payment.ReviewFeedback,
                ConsumedAt = payment.ActivatedAt,
                ScreenshotUrl = ScreenshotRoute(payment.Id)
            };
        }).ToList();
    }

    private async Task RunSideEffectAsync(Func<Task> effect, string name, Guid paymentId)
    {
        try
        {
            await effect();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Ad payment {PaymentId}: the {SideEffect} failed.", paymentId, name);
        }
    }

    private static ListingModuleType TypeOf(Payment payment) =>
        Enum.Parse<ListingModuleType>(payment.TargetType!);

    private static AdPaymentMethodDto ToDto(PaymentMethod method) => new()
    {
        Id = method.Id,
        Name = method.Name,
        ArabicName = method.ArabicName,
        Type = method.Type,
        PhoneNumber = method.PhoneNumber,
        InstaPayIdentifier = method.InstaPayIdentifier,
        BankName = method.BankName,
        AccountHolderName = method.AccountHolderName,
        AccountNumber = method.AccountNumber,
        Iban = method.Iban,
        PaymentInfo = PaymentCatalog.ResolvePaymentInfo(
            method.Type, method.PhoneNumber, method.InstaPayIdentifier, method.AccountNumber),
        Instructions = method.Instructions
    };

    internal static IReadOnlyList<int> ParseIds(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? Array.Empty<int>()
            : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(part => int.TryParse(part, out var id) ? id : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();

    private static string Describe(AdPaymentSetting settings) =>
        $"enabled={settings.IsEnabled}; price={settings.Price:0.##} {settings.Currency}; methods={settings.PaymentMethodIds ?? "all"}";

    private static string ApprovedMessage(string? title, string action) => action switch
    {
        AdPaymentListingActions.Republished =>
            $"✅ تم تأكيد دفع رسوم إعلان «{title}» وتمت إعادة نشره لمدة {ListingLifecycle.ActiveDurationDays} يوم.",
        AdPaymentListingActions.AwaitingModeration =>
            $"✅ تم تأكيد دفع رسوم إعلان «{title}». الإعلان دلوقتي في انتظار مراجعة الإدارة قبل النشر.",
        _ => $"✅ تم تأكيد دفع رسوم إعلان «{title}»."
    };

    private static string RejectedMessage(string? title, string? reason, string? feedback) =>
        $"❌ تم رفض إثبات دفع رسوم إعلان «{title}»." +
        (reason is null ? string.Empty : $" السبب: {reason}.") +
        FeedbackSuffix(feedback) +
        " تقدر ترفع إثبات دفع جديد من صفحة الإعلان.";

    private static string FeedbackSuffix(string? feedback) =>
        feedback is null ? string.Empty : $" ملاحظات الإدارة: {feedback}";

    private static string ActionName(string action) => action switch
    {
        AdPaymentListingActions.Republished => "تمت إعادة نشر الإعلان",
        AdPaymentListingActions.AwaitingModeration => "الإعلان في انتظار مراجعة الإدارة",
        AdPaymentListingActions.CreditKept => "تم حفظ الدفع للإعلان لحين نشره",
        _ => "لا يوجد إجراء على الإعلان"
    };
}
