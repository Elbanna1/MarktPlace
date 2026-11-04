using Domain.Entities;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Services.AdForms;
using Services.Listings;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Listings;
using Shared.DTOs.Lookups.Forms;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.Admin;

public class AdminAdService : IAdminAdService
{
    private readonly IAdminAdRepository _repository;
    private readonly IListingInteractionService _interactions;
    private readonly ILookupService _lookups;
    private readonly INotificationService _notifications;
    private readonly IListingInterestNotifier _interestNotifier;
    private readonly IAdminAuditService _audit;
    private readonly ILogger<AdminAdService> _logger;
    private readonly IAdPaymentGate? _paymentGate;

    public AdminAdService(
        IAdminAdRepository repository,
        IListingInteractionService interactions,
        ILookupService lookups,
        INotificationService notifications,
        IListingInterestNotifier interestNotifier,
        IAdminAuditService audit,
        ILogger<AdminAdService> logger,
        IAdPaymentGate? paymentGate = null)
    {
        _repository = repository;
        _interactions = interactions;
        _lookups = lookups;
        _notifications = notifications;
        _interestNotifier = interestNotifier;
        _audit = audit;
        _logger = logger;
        _paymentGate = paymentGate;
    }

    public Task<PaginatedResult<AdminAdListItemDto>> GetAdsAsync(
        AdminAdFilterParams filter, CancellationToken cancellationToken = default) =>
        GetAdsAsync(filter, includeTotal: true, cancellationToken);

    public Task<PaginatedResult<AdminAdListItemDto>> GetExpiredAdsAsync(
        AdminAdFilterParams filter, CancellationToken cancellationToken = default)
    {
        filter.Status = ListingStatus.Expired;
        filter.ModerationStatus = ModerationStatus.Approved;

        return GetAdsAsync(filter, includeTotal: true, cancellationToken, orderByExpiry: true);
    }

    public Task<int> GetExpiredCountAsync(CancellationToken cancellationToken = default) =>
        _repository.CountAsync(
            new AdminAdFilterParams
            {
                Status = ListingStatus.Expired,
                ModerationStatus = ModerationStatus.Approved
            },
            DateTime.UtcNow,
            cancellationToken);

    public async Task<IReadOnlyList<AdminAdListItemDto>> GetLatestAdsAsync(
        int count, CancellationToken cancellationToken = default)
    {
        var page = await GetAdsAsync(
            new AdminAdFilterParams { PageIndex = 1, PageSize = count },
            includeTotal: false,
            cancellationToken);

        return page.Items;
    }

    private async Task<PaginatedResult<AdminAdListItemDto>> GetAdsAsync(
        AdminAdFilterParams filter, bool includeTotal, CancellationToken cancellationToken,
        bool orderByExpiry = false)
    {
        var utcNow = DateTime.UtcNow;

        var (rows, totalCount) = await _repository.GetPagedAsync(
            filter, utcNow, cancellationToken, includeTotal, orderByExpiry);

        if (rows.Count == 0)
            return new PaginatedResult<AdminAdListItemDto>(
                Array.Empty<AdminAdListItemDto>(), totalCount, filter.PageIndex, filter.PageSize);

        var owners = await _repository.GetOwnersAsync(
            rows.Select(row => row.OwnerId).Distinct().ToList(), cancellationToken);

        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        var payments = _paymentGate is null
            ? null
            : await _paymentGate.DescribeAsync(
                rows.Select(row => new AdPaymentListingSnapshot(
                        row.Type, row.Id, row.CreatedAt, row.ModerationStatus,
                        AwaitsFirstWindow(row.ModerationStatus, row.ExpireAt, utcNow)))
                    .ToList(),
                utcNow, cancellationToken);

        var items = rows.Select(row =>
        {
            var (categoryId, subCategoryId) = ListingInteractionService.ResolveCategory(row);
            var (categoryName, subCategoryName) =
                ListingInteractionService.ResolveNames(tree, categoryId, subCategoryId);

            var owner = owners.GetValueOrDefault(row.OwnerId);
            var route = ListingModuleCatalog.RouteOf(row.Type);

            return new AdminAdListItemDto
            {
                Id = row.Id,
                Type = row.Type.ToString(),
                TypeId = row.Type,
                Route = route,
                DetailsEndpoint = DetailsEndpointOf(route, row.Id),
                Title = row.Title,
                MainImageUrl = row.MainImageUrl,
                Price = row.Price,
                CategoryId = categoryId,
                CategoryName = categoryName,
                SubCategoryId = subCategoryId,
                SubCategoryName = subCategoryName,
                OwnerId = row.OwnerId,
                OwnerName = string.IsNullOrWhiteSpace(owner?.Name) ? null : owner!.Name,
                OwnerPhone = owner?.PhoneNumber,
                Status = row.Status.ToString(),

                Moderation = ListingModerationDto.From(row),
                CreatedAt = row.CreatedAt,
                StartDate = row.PublishedAt,
                EndDate = row.ExpireAt,
                ExpireAt = row.ExpireAt,

                RemainingDays = row.Status == ListingStatus.Expired
                    ? 0
                    : ListingLifecycle.RemainingDays(row.ExpireAt, utcNow),

                IsExpired = row.Status == ListingStatus.Expired,
                IsExpiringSoon = row.Status == ListingStatus.Active &&
                                 AdminRepublishPolicy.IsExpiringSoon(row.ExpireAt, utcNow),
                CanRepublish = AdminRepublishPolicy.CanRepublish(
                                   row.Type, row.ModerationStatus, row.ExpireAt, utcNow) ||
                               (row.ModerationStatus == ModerationStatus.Approved &&
                                row.Status == ListingStatus.Expired),
                RepublishCount = row.RepublishCount,
                Payment = payments?.GetValueOrDefault((row.Type, row.Id))
            };
        }).ToList();

        return new PaginatedResult<AdminAdListItemDto>(
            items, totalCount, filter.PageIndex, filter.PageSize);
    }

    public Task<int> GetPendingCountAsync(CancellationToken cancellationToken = default) =>
        _repository.CountByStatusAsync(ModerationStatus.Pending, DateTime.UtcNow, cancellationToken);

    public async Task<AdminAdDetailsDto> GetAdDetailsAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default)
    {
        var snapshot = await _repository.LoadListingAsync(type, id, cancellationToken)
            ?? throw new NotFoundException("الإعلان غير موجود.");

        var listing = snapshot.Listing;
        var values = snapshot.Values;

        var subCategoryId = ReadInt(values, "SubCategoryId")
            ?? (int?)ListingModuleCatalog.SubCategoryOf(type)
            ?? 0;

        var categoryId = ReadInt(values, "CategoryId")
            ?? (int?)ListingModuleCatalog.CategoryOf(type)
            ?? 0;

        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);
        var (categoryName, subCategoryName) =
            ListingInteractionService.ResolveNames(tree, categoryId, subCategoryId);

        var owners = await _repository.GetOwnersAsync(new[] { listing.OwnerUserId }, cancellationToken);
        var owner = owners.GetValueOrDefault(listing.OwnerUserId);

        var route = ListingModuleCatalog.RouteOf(type);

        var schema = AdFormSchemaCatalog.GetSchema(categoryId, subCategoryId);
        var fields = BuildFields(values, schema?.Fields);

        var utcNow = DateTime.UtcNow;
        var expireAt = ReadDate(values, "ExpireAt");
        var publishedAt = ReadDate(values, "PublishedAt");
        var createdAt = ReadDate(values, "CreatedAt") ?? DateTime.UtcNow;

        var isApproved = listing.ModerationStatus == ModerationStatus.Approved;
        var isExpired = isApproved &&
                        (ListingLifecycle.HasExpired(expireAt, utcNow) || IsStampedExpired(listing));

        return new AdminAdDetailsDto
        {
            Id = id,
            Type = type.ToString(),
            TypeId = type,
            TypeName = ListingModuleCatalog.NameOf(type),
            Route = route,
            DetailsEndpoint = DetailsEndpointOf(route, id),

            Title = listing.ListingTitle,
            Price = ReadDecimal(values, "Price"),
            CategoryId = categoryId,
            CategoryName = categoryName,
            SubCategoryId = subCategoryId,
            SubCategoryName = subCategoryName,

            OwnerId = listing.OwnerUserId,
            OwnerName = string.IsNullOrWhiteSpace(owner?.Name) ? null : owner!.Name,
            OwnerPhone = owner?.PhoneNumber,

            Status = (isExpired ? ListingStatus.Expired : ResolveStatus(listing.ModerationStatus, expireAt)).ToString(),
            Moderation = new ListingModerationDto
            {
                Status = listing.ModerationStatus.ToString(),
                StatusName = ModerationCatalog.NameOf(listing.ModerationStatus),
                Reason = listing.RejectionReason,
                ReasonName = listing.RejectionReason is { } reason
                    ? ModerationCatalog.NameOf(reason)
                    : null,
                Notes = listing.ModerationNotes,
                DecidedAt = listing.ModeratedAt
            },

            CreatedAt = createdAt,
            StartDate = publishedAt,
            EndDate = expireAt,
            ExpireAt = expireAt,
            RemainingDays = isExpired ? 0 : ListingLifecycle.RemainingDays(expireAt, utcNow),

            IsExpired = isExpired,
            IsExpiringSoon = isApproved && !isExpired &&
                             AdminRepublishPolicy.IsExpiringSoon(expireAt, utcNow),
            CanRepublish = isExpired ||
                           AdminRepublishPolicy.CanRepublish(type, listing.ModerationStatus, expireAt, utcNow),
            RepublishCount = ReadInt(values, "RepublishCount") ?? 0,

            Payment = _paymentGate is null
                ? null
                : (await _paymentGate.DescribeAsync(
                    new[]
                    {
                        new AdPaymentListingSnapshot(
                            type, id, ReadDate(values, "CreatedAt"), listing.ModerationStatus,
                            listing.ModerationStatus is ModerationStatus.Pending or ModerationStatus.Rejected &&
                            Services.AdPayments.AdPaymentRules.OpensNewWindow(listing, utcNow))
                    },
                    utcNow, cancellationToken)).GetValueOrDefault((type, id)),

            ImageUrls = snapshot.ImageUrls,
            VideoUrls = snapshot.VideoUrls,

            Fields = fields,
            Raw = values
        };
    }

    public async Task DeleteAsync(
        ListingModuleType type, Guid id, string adminUserId, CancellationToken cancellationToken = default)
    {
        var listing = await LoadAsync(type, id, cancellationToken);

        var ownerId = listing.OwnerUserId;
        var title = listing.ListingTitle;
        var previousStatus = listing.ModerationStatus;

        var deleted = await _repository.DeleteListingAsync(type, id, cancellationToken);

        if (!deleted)
            throw new NotFoundException("الإعلان غير موجود.");

        await _interactions.PurgeListingAsync(type, id, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.DeleteAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"حذف إعلان: {title}",
            oldValue: ModerationCatalog.NameOf(previousStatus), newValue: "محذوف",
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyAsync(
            ownerId,
            NotificationCatalog.Moderation.DeletedByAdmin(type, id, title),
            NotificationAction.Deleted,
            id,
            type);
    }

    private static bool AwaitsFirstWindow(ModerationStatus status, DateTime? expireAt, DateTime utcNow) =>
        status is ModerationStatus.Pending or ModerationStatus.Rejected &&
        !(expireAt is { } end && end > utcNow);

    public async Task<AdminModerationResultDto> ApproveAsync(
        ListingModuleType type, Guid id, string adminUserId,
        ApproveListingRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await LoadAsync(type, id, cancellationToken);

        var previousStatus = listing.ModerationStatus;
        var previousExpireAt = WindowEndOf(listing);

        var opensWindow = Services.AdPayments.AdPaymentRules.OpensNewWindow(listing, DateTime.UtcNow);

        if (opensWindow && _paymentGate is not null)
            await _paymentGate.EnsurePaidAsync(type, id, _repository.CreatedAtOf(listing), cancellationToken);

        Decide(listing, ModerationStatus.Approved, adminUserId, reason: null, request.Notes);

        await _repository.SaveModerationAsync(type, cancellationToken);

        if (opensWindow && _paymentGate is not null && WindowEndOf(listing) != previousExpireAt)
            await _paymentGate.ConsumeAsync(type, id, DateTime.UtcNow, WindowEndOf(listing), cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.ApproveAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"قبول إعلان: {listing.ListingTitle}",
            oldValue: ModerationCatalog.NameOf(previousStatus),
            newValue: ModerationCatalog.NameOf(ModerationStatus.Approved),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        if (previousStatus == ModerationStatus.Approved && WindowEndOf(listing) == previousExpireAt)
        {
            _logger.LogInformation(
                "{Module} {ListingId} was already approved; the repeated approval sends no second notification.",
                type, id);

            return ToResult(type, id, listing);
        }

        await NotifyAsync(
            listing.OwnerUserId,
            NotificationCatalog.Moderation.Approved(type, id, listing.ListingTitle),
            NotificationAction.Approved,
            id,
            type);

        try
        {
            await _interestNotifier.AnnounceAsync(type, id, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                "The approval of {Module} {ListingId} succeeded but the interest fan-out failed.",
                type, id);
        }

        return ToResult(type, id, listing);
    }

    public async Task<AdminModerationResultDto> RejectAsync(
        ListingModuleType type, Guid id, string adminUserId,
        RejectListingRequest request, CancellationToken cancellationToken = default)
    {
        if (ModerationCatalog.RequiresNotes(request.Reason) && string.IsNullOrWhiteSpace(request.Notes))
            throw new BadRequestException("يجب كتابة سبب الرفض عند اختيار \"سبب آخر\".");

        var listing = await LoadAsync(type, id, cancellationToken);

        var previousStatus = listing.ModerationStatus;

        Decide(listing, ModerationStatus.Rejected, adminUserId, request.Reason, request.Notes);

        await _repository.SaveModerationAsync(type, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.RejectAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"رفض إعلان: {listing.ListingTitle} — {ModerationCatalog.NameOf(request.Reason)}",
            oldValue: ModerationCatalog.NameOf(previousStatus),
            newValue: ModerationCatalog.NameOf(ModerationStatus.Rejected),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        if (previousStatus == ModerationStatus.Rejected)
            return ToResult(type, id, listing);

        await NotifyAsync(
            listing.OwnerUserId,
            NotificationCatalog.Moderation.Rejected(type, id, listing.ListingTitle, request.Reason, request.Notes),
            NotificationAction.Rejected,
            id,
            type);

        return ToResult(type, id, listing);
    }

    public async Task<AdminModerationResultDto> SuspendAsync(
        ListingModuleType type, Guid id, string adminUserId,
        SuspendListingRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await LoadAsync(type, id, cancellationToken);

        var previousStatus = listing.ModerationStatus;

        Decide(listing, ModerationStatus.Suspended, adminUserId, reason: null, request.Notes);

        await _repository.SaveModerationAsync(type, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.SuspendAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"إيقاف إعلان: {listing.ListingTitle}",
            oldValue: ModerationCatalog.NameOf(previousStatus),
            newValue: ModerationCatalog.NameOf(ModerationStatus.Suspended),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        if (previousStatus == ModerationStatus.Suspended)
            return ToResult(type, id, listing);

        await NotifyAsync(
            listing.OwnerUserId,
            NotificationCatalog.Moderation.Suspended(type, id, listing.ListingTitle, request.Notes),
            NotificationAction.Suspended,
            id,
            type);

        return ToResult(type, id, listing);
    }

    public async Task<AdminAdMetadataDto> GetMetadataAsync(CancellationToken cancellationToken = default)
    {
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        return new AdminAdMetadataDto
        {
            ModerationStatuses = ModerationCatalog.AllStatuses
                .Select(status => new AdminOptionDto
                {
                    Id = (int)status,
                    Name = ModerationCatalog.NameOf(status)
                })
                .ToList(),

            RejectionReasons = ModerationCatalog.AllRejectionReasons
                .Select(reason => new AdminOptionDto
                {
                    Id = (int)reason,
                    Name = ModerationCatalog.NameOf(reason),
                    RequiresNotes = ModerationCatalog.RequiresNotes(reason)
                })
                .ToList(),

            Modules = ListingModuleCatalog.All
                .Select(module =>
                {
                    var subCategory = ListingModuleCatalog.SubCategoryOf(module);
                    var category = subCategory is { } sub ? ListingModuleCatalog.CategoryOf(sub) : null;

                    var categoryId = (int)(category ?? 0);
                    var subCategoryId = (int)(subCategory ?? 0);

                    var (categoryName, subCategoryName) =
                        ListingInteractionService.ResolveNames(tree, categoryId, subCategoryId);

                    return new AdminModuleOptionDto
                    {
                        Id = module,
                        Name = module.ToString(),
                        ArabicName = ListingModuleCatalog.NameOf(module),
                        Route = ListingModuleCatalog.RouteOf(module),
                        CategoryId = categoryId,
                        CategoryName = categoryName,
                        SubCategoryId = subCategoryId,
                        SubCategoryName = subCategoryName
                    };
                })
                .ToList()
        };
    }

    private static IReadOnlyList<AdminAdFieldValueDto> BuildFields(
        IReadOnlyDictionary<string, object?> values,
        IReadOnlyList<FormFieldDto>? formFields)
    {
        var fields = new List<AdminAdFieldValueDto>(values.Count);
        var claimed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var formField in formFields ?? Array.Empty<FormFieldDto>())
        {
            if (!values.TryGetValue(formField.Name, out var value))
                continue;

            claimed.Add(formField.Name);

            fields.Add(new AdminAdFieldValueDto
            {
                Name = formField.Name,
                Label = formField.Label,
                Type = formField.Type,
                Value = value,
                DisplayValue = ResolveDisplayValue(formField, value),
                InForm = true
            });
        }

        foreach (var (name, value) in values)
        {
            if (claimed.Contains(name))
                continue;

            fields.Add(new AdminAdFieldValueDto
            {
                Name = name,
                Label = name,
                Type = null,
                Value = value,
                InForm = false
            });
        }

        return fields;
    }

    private static string? ResolveDisplayValue(FormFieldDto field, object? value)
    {
        if (value is null || field.Options is not { Count: > 0 })
            return null;

        var text = Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);

        var match = field.Options.FirstOrDefault(option =>
            string.Equals(
                Convert.ToString(option.Value, System.Globalization.CultureInfo.InvariantCulture),
                text,
                StringComparison.OrdinalIgnoreCase));

        return match?.Label;
    }

    private static ListingStatus ResolveStatus(ModerationStatus moderation, DateTime? expireAt) =>
        moderation switch
        {
            ModerationStatus.Pending => ListingStatus.Pending,
            ModerationStatus.Rejected => ListingStatus.Rejected,
            ModerationStatus.Suspended => ListingStatus.Suspended,
            _ => expireAt is { } expiry && expiry <= DateTime.UtcNow
                ? ListingStatus.Expired
                : ListingStatus.Active
        };

    public async Task<AdminRepublishResultDto> RepublishAsync(
        ListingModuleType type, Guid id, string adminUserId, CancellationToken cancellationToken = default)
    {
        if (!AdminRepublishPolicy.Supports(type))
            throw new BadRequestException("إعادة النشر من الإدارة غير متاحة لهذا القسم.");

        var (listing, previousExpireAt, publishedAt, expireAt, republishCount) =
            await _repository.RunExclusiveAsync(type, id, async () =>
            {
                var found = await LoadAsync(type, id, cancellationToken);

                if (found is Advertisement { Status: AdvertisementStatus.Deleted } or Advertisement { DeletedAt: not null })
                    throw new NotFoundException("الإعلان غير موجود.");

                if (found.ModerationStatus != ModerationStatus.Approved)
                    throw new BadRequestException(
                        $"لا يمكن إعادة نشر إعلان حالته «{ModerationCatalog.NameOf(found.ModerationStatus)}». " +
                        "إعادة النشر متاحة للإعلانات المعتمدة فقط.");

                var now = DateTime.UtcNow;
                var previous = WindowEndOf(found);

                if (previous is null)
                    throw new BadRequestException("لم يتم نشر هذا الإعلان بعد، ولذلك لا يحتاج إلى إعادة نشر.");

                if (!AdminRepublishPolicy.IsWindowOpen(previous, now) && !IsStampedExpired(found))
                {
                    var left = ListingLifecycle.RemainingDays(previous, now) ?? 0;

                    throw new BadRequestException(
                        $"إعادة النشر بتكون متاحة قبل انتهاء الإعلان بـ {AdminRepublishPolicy.WarningDays} أيام أو بعد انتهائه. " +
                        $"الإعلان لسه فاضل له {left} يوم.");
                }

                StartWindow(found, now);
                var count = CountRepublish(found);

                found.ModeratedAt = now;
                found.ModeratedBy = adminUserId;

                _repository.MarkAsNew(found, now);

                await _repository.SaveModerationAsync(type, cancellationToken);

                return (found, previous.Value, now, WindowEndOf(found)!.Value, count);
            }, cancellationToken);

        if (_paymentGate is not null)
            await _paymentGate.ConsumeAsync(type, id, publishedAt, expireAt, cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.RepublishAd, AdminAuditCatalog.Targets.Advertisement, id.ToString(),
            $"إعادة نشر إعلان: {listing.ListingTitle}",
            oldValue: previousExpireAt.ToString("O"),
            newValue: expireAt.ToString("O"),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyAsync(
            listing.OwnerUserId,
            NotificationCatalog.Build(NotificationCatalog.For(type), NotificationAction.Republished, id, listing.ListingTitle),
            NotificationAction.Republished,
            id,
            type);

        var moderation = ToResult(type, id, listing).Moderation;

        return new AdminRepublishResultDto
        {
            Id = id,
            TypeId = type,
            Type = type.ToString(),
            Title = listing.ListingTitle,
            PublishedAt = publishedAt,
            ExpireAt = expireAt,
            PreviousExpireAt = previousExpireAt,
            RemainingDays = ListingLifecycle.RemainingDays(expireAt, publishedAt) ?? 0,
            RepublishCount = republishCount,
            Status = ListingStatus.Active.ToString(),
            Moderation = moderation
        };
    }
    private static int? ReadInt(IReadOnlyDictionary<string, object?> values, string name) =>
        values.TryGetValue(name, out var value) && value is int number ? number : null;

    private static decimal? ReadDecimal(IReadOnlyDictionary<string, object?> values, string name) =>
        values.TryGetValue(name, out var value) && value is decimal number ? number : null;

    private static DateTime? ReadDate(IReadOnlyDictionary<string, object?> values, string name) =>
        values.TryGetValue(name, out var value) && value is DateTime date ? date : null;

    private async Task<IModeratedListing> LoadAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken) =>
        await _repository.FindForModerationAsync(type, id, cancellationToken)
        ?? throw new NotFoundException("الإعلان غير موجود.");

    private static void Decide(
        IModeratedListing listing,
        ModerationStatus status,
        string adminUserId,
        ListingRejectionReason? reason,
        string? notes)
    {
        var now = DateTime.UtcNow;

        listing.ModerationStatus = status;
        listing.ModeratedAt = now;
        listing.ModeratedBy = adminUserId;
        listing.RejectionReason = status == ModerationStatus.Rejected ? reason : null;
        listing.ModerationNotes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();

        if (status == ModerationStatus.Approved)
            OpenWindow(listing, now);
    }

    private static DateTime? WindowEndOf(IModeratedListing listing) => listing switch
    {
        IExpiringListing expiring => expiring.ExpireAt,
        Advertisement advertisement => advertisement.ExpireAt,
        _ => null
    };

    private static bool IsStampedExpired(IModeratedListing listing) =>
        listing is Advertisement { Status: AdvertisementStatus.Expired };

    private static void OpenWindow(IModeratedListing listing, DateTime now)
    {
        if (WindowEndOf(listing) is { } current && current > now && !IsStampedExpired(listing))
            return;

        StartWindow(listing, now);
    }

    private static void StartWindow(IModeratedListing listing, DateTime now)
    {
        switch (listing)
        {
            case IExpiringListing expiring:
                expiring.PublishedAt = now;
                expiring.ExpireAt = ListingLifecycle.EndOf(now);
                expiring.FirstPublishedAt ??= now;
                break;

            case Advertisement advertisement:
                advertisement.PublishedAt = now;
                advertisement.ExpireAt = ListingLifecycle.EndOf(now);
                advertisement.FirstPublishedAt ??= now;

                if (advertisement.Status == AdvertisementStatus.Expired)
                    advertisement.Status = AdvertisementStatus.Active;

                advertisement.ExpiredAt = null;
                break;
        }
    }

    private static int CountRepublish(IModeratedListing listing) => listing switch
    {
        IExpiringListing expiring => ++expiring.RepublishCount,
        Advertisement advertisement => ++advertisement.RepublishCount,
        _ => 0
    };
    private static AdminModerationResultDto ToResult(
        ListingModuleType type, Guid id, IModeratedListing listing) => new()
    {
        Id = id,
        TypeId = type,
        Type = type.ToString(),
        Title = listing.ListingTitle,
        OwnerId = listing.OwnerUserId,
        Moderation = new ListingModerationDto
        {
            Status = listing.ModerationStatus.ToString(),
            StatusName = ModerationCatalog.NameOf(listing.ModerationStatus),
            Reason = listing.RejectionReason,
            ReasonName = listing.RejectionReason is { } reason
                ? ModerationCatalog.NameOf(reason)
                : null,
            Notes = listing.ModerationNotes,
            DecidedAt = listing.ModeratedAt
        }
    };

    private static string DetailsEndpointOf(string route, Guid id)
    {
        if (string.IsNullOrEmpty(route))
            return string.Empty;

        var firstSegment = route.Split('/', StringSplitOptions.RemoveEmptyEntries)[0];

        return $"/api/{firstSegment}/{id}";
    }

    private Task NotifyAsync(
        string userId, NotificationContent content, NotificationAction action, Guid listingId,
        ListingModuleType type) =>
        _notifications.CreateAsync(
            userId,
            content.WithListing(
                listingType: type,
                categoryId: (int?)ListingModuleCatalog.CategoryOf(type),
                subCategoryId: (int?)ListingModuleCatalog.SubCategoryOf(type)),
            listingId,
            action);
}
