using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Advertisements;
using Shared.DTOs.Payments;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly INotificationService _notifications;
    private readonly IAdminAuditService _audit;
    private readonly IAdminAlertService _alerts;

    public PaymentService(
        IPaymentRepository repository,
        IFileService fileService,
        IMapper mapper,
        INotificationService notifications,
        IAdminAuditService audit,
        IAdminAlertService alerts)
    {
        _repository = repository;
        _fileService = fileService;
        _mapper = mapper;
        _notifications = notifications;
        _audit = audit;
        _alerts = alerts;
    }

    public async Task<IReadOnlyList<PaymentMethodDto>> GetMethodsAsync(CancellationToken cancellationToken = default)
    {
        var methods = await _repository.GetMethodsAsync(activeOnly: true, cancellationToken);
        return _mapper.Map<IReadOnlyList<PaymentMethodDto>>(methods);
    }

    public async Task<IReadOnlyList<PaymentListItemDto>> GetMyPaymentsAsync(
        string userId, PaymentStatus? status = null, CancellationToken cancellationToken = default)
    {
        var payments = await _repository.GetByUserAsync(userId, status, cancellationToken);
        return _mapper.Map<IReadOnlyList<PaymentListItemDto>>(payments);
    }

    public async Task<PaymentDetailsDto> GetByIdAsync(
        Guid id, string userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var payment = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("عملية الدفع مش موجودة.");

        if (!isAdmin && !string.Equals(payment.UserId, userId, StringComparison.Ordinal))
            throw new NotFoundException("عملية الدفع مش موجودة.");

        return _mapper.Map<PaymentDetailsDto>(payment);
    }

    public async Task<PaginatedResult<AdminPaymentListItemDto>> GetAllAsync(
        AdminPaymentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (payments, totalCount) = await _repository.GetPagedForAdminAsync(filter, cancellationToken);

        var items = payments.Select(MapForAdmin).ToList();

        return new PaginatedResult<AdminPaymentListItemDto>(
            items, totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<AdminPaymentDetailsDto> GetForAdminAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        var payment = await _repository.GetForAdminAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("عملية الدفع غير موجودة.");

        return await BuildAdminDetailsAsync(payment, cancellationToken);
    }

    public async Task<AdminPaymentDetailsDto> ApprovePaymentAsync(
        Guid id, string adminUserId, CancellationToken cancellationToken = default)
    {
        var payment = await LoadForDecisionAsync(id, cancellationToken);

        payment.Status = PaymentStatus.Approved;
        payment.ApprovedAt = DateTime.UtcNow;
        payment.ApprovedBy = adminUserId;
        payment.RejectedAt = null;
        payment.RejectReason = null;

        _repository.Update(payment);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.ApprovePayment, AdminAuditCatalog.Targets.Payment, id.ToString(),
            $"تأكيد عملية دفع بقيمة {payment.Amount:N0} {payment.Currency}",
            oldValue: PaymentCatalog.GetStatusName(PaymentStatus.Pending),
            newValue: PaymentCatalog.GetStatusName(PaymentStatus.Approved),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyPayerAsync(
            payment,
            "تأكيد الدفع",
            "✅ تم تأكيد استلام المبلغ.",
            "✅");

        return await GetForAdminAsync(id, cancellationToken);
    }

    public async Task<AdminPaymentDetailsDto> RejectPaymentAsync(
        Guid id, string adminUserId, RejectPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var payment = await LoadForDecisionAsync(id, cancellationToken);

        var reason = request.Reason.Trim();

        payment.Status = PaymentStatus.Rejected;
        payment.RejectedAt = DateTime.UtcNow;
        payment.ApprovedBy = adminUserId;
        payment.ApprovedAt = null;
        payment.RejectReason = reason;

        _repository.Update(payment);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.RejectPayment, AdminAuditCatalog.Targets.Payment, id.ToString(),
            $"رفض عملية دفع بقيمة {payment.Amount:N0} {payment.Currency} — {reason}",
            oldValue: PaymentCatalog.GetStatusName(PaymentStatus.Pending),
            newValue: PaymentCatalog.GetStatusName(PaymentStatus.Rejected),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyPayerAsync(
            payment,
            "رفض الدفع",
            $"❌ لم يتم قبول إثبات الدفع. السبب: {reason}",
            "❌");

        return await GetForAdminAsync(id, cancellationToken);
    }

    public async Task<AdminPaymentDetailsDto> RefundPaymentAsync(
        Guid id, string adminUserId, RefundPaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var payment = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("عملية الدفع غير موجودة.");

        if (payment.Status != PaymentStatus.Approved)
            throw new BadRequestException("لا يمكن استرداد إلا عملية دفع مؤكدة.");

        var reason = request.Reason.Trim();

        payment.Status = PaymentStatus.Refunded;
        payment.RejectReason = reason;
        payment.ApprovedBy = adminUserId;

        _repository.Update(payment);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.RefundPayment, AdminAuditCatalog.Targets.Payment, id.ToString(),
            $"استرداد عملية دفع بقيمة {payment.Amount:N0} {payment.Currency} — {reason}",
            oldValue: PaymentCatalog.GetStatusName(PaymentStatus.Approved),
            newValue: PaymentCatalog.GetStatusName(PaymentStatus.Refunded),
            adminUserId: adminUserId, cancellationToken: cancellationToken);

        await NotifyPayerAsync(
            payment,
            "استرداد المبلغ",
            $"↩️ تم استرداد المبلغ. السبب: {reason}",
            "↩️");

        return await GetForAdminAsync(id, cancellationToken);
    }

    private async Task<Payment> LoadForDecisionAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("عملية الدفع غير موجودة.");

        if (payment.Status != PaymentStatus.Pending)
            throw new BadRequestException(
                $"تم البت في عملية الدفع بالفعل ({PaymentCatalog.GetStatusName(payment.Status)}).");

        return payment;
    }

    private Task NotifyPayerAsync(Payment payment, string title, string message, string icon) =>
        _notifications.CreateAsync(
            payment.UserId,
            title,
            message,
            NotificationType.PaymentSubmitted,
            payment.Id,
            NotificationReferenceTypes.Payment,
            NotificationAction.Updated,
            icon,
            "عملية الدفع");

    private async Task<AdminPaymentDetailsDto> BuildAdminDetailsAsync(
        Payment payment, CancellationToken cancellationToken)
    {
        var details = new AdminPaymentDetailsDto
        {
            Notes = payment.Notes,
            CurrentPaymentMethodName = payment.PaymentMethod?.Name,
            ReviewedBy = payment.ApprovedBy,
            ReviewedByName = payment.Reviewer is null
                ? null
                : $"{payment.Reviewer.FirstName} {payment.Reviewer.SecondName}".Trim(),
            RejectReason = payment.RejectReason,
            Snapshot = BuildSnapshot(payment)
        };

        CopyListFields(payment, details);

        var booking = await _repository.FindTargetBannerBookingAsync(payment, cancellationToken);

        if (booking is not null)
        {
            details.BannerRequest = new AdminDashboardBannerRequestDto
            {
                Id = booking.Id,
                AdvertiserName = booking.AdvertiserName,
                Title = booking.Title,
                Location = booking.Location,
                LocationName = BannerBookingCatalog.GetLocationName(booking.Location),
                SlotNumber = booking.SlotNumber,
                Price = booking.Price,
                Currency = booking.Currency,
                PaymentStatus = booking.PaymentStatus,
                PaymentStatusName = BannerBookingCatalog.GetPaymentStatusName(booking.PaymentStatus),
                Status = booking.Status,
                StatusName = BannerBookingCatalog.GetStatusName(booking.Status),
                SubmittedAt = booking.SubmittedAt
            };
        }

        return details;
    }

    private static AdminPaymentSnapshotDto BuildSnapshot(Payment payment)
    {
        var hasSnapshot = !string.IsNullOrWhiteSpace(payment.PaymentMethodNameSnapshot) ||
                          !string.IsNullOrWhiteSpace(payment.PaymentInfoSnapshot);

        if (hasSnapshot)
        {
            return new AdminPaymentSnapshotDto
            {
                PaymentMethodName = payment.PaymentMethodNameSnapshot,
                PaymentInfo = payment.PaymentInfoSnapshot,
                AccountName = payment.AccountNameSnapshot,
                Instructions = payment.InstructionsSnapshot,
                IsSnapshot = true
            };
        }

        var method = payment.PaymentMethod;

        return new AdminPaymentSnapshotDto
        {
            PaymentMethodName = method?.Name,
            PaymentInfo = method is null
                ? null
                : PaymentCatalog.ResolvePaymentInfo(
                    method.Type, method.PhoneNumber, method.InstaPayIdentifier, method.AccountNumber),
            AccountName = method?.AccountHolderName,
            Instructions = method?.Instructions,
            IsSnapshot = false
        };
    }

    private static AdminPaymentListItemDto MapForAdmin(Payment payment)
    {
        var row = new AdminPaymentListItemDto();

        CopyListFields(payment, row);

        return row;
    }

    private static void CopyListFields(Payment payment, AdminPaymentListItemDto row)
    {
        row.Id = payment.Id;
        row.UserId = payment.UserId;
        row.UserName = payment.User is null
            ? null
            : $"{payment.User.FirstName} {payment.User.SecondName}".Trim();
        row.UserPhone = payment.User?.PhoneNumber;
        row.UserEmail = payment.User?.Email;
        row.Amount = payment.Amount;
        row.Currency = payment.Currency;
        row.PaymentMethodId = payment.PaymentMethodId;

        row.PaymentMethodName = payment.PaymentMethodNameSnapshot
            ?? payment.PaymentMethod?.Name
            ?? string.Empty;

        row.ScreenshotUrl = payment.ScreenshotUrl;
        row.Status = payment.Status;
        row.StatusName = PaymentCatalog.GetStatusName(payment.Status);
        row.SubmittedAt = payment.SubmittedAt;
        row.ReviewedAt = payment.ApprovedAt ?? payment.RejectedAt;
    }

    public async Task<PaymentDetailsDto> SubmitAsync(
        string userId,
        SubmitPaymentRequest request,
        UploadImageModel? screenshot,
        CancellationToken cancellationToken = default)
    {
        if (screenshot is null)
            throw new BadRequestException("صورة إيصال الدفع مطلوبة.");

        var method = await _repository.GetMethodByIdAsync(request.PaymentMethodId, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("طريقة الدفع مش موجودة.");

        if (!method.IsActive)
            throw new BadRequestException($"طريقة الدفع '{method.Name}' مابقتش متاحة.");

        var stored = await _fileService.SaveAsync(screenshot, ImageConstants.PaymentsFolder, cancellationToken);

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PaymentMethodId = method.Id,

            PaymentMethodNameSnapshot = method.Name,
            PaymentInfoSnapshot = PaymentCatalog.ResolvePaymentInfo(
                method.Type, method.PhoneNumber, method.InstaPayIdentifier, method.AccountNumber),
            AccountNameSnapshot = method.AccountHolderName,
            InstructionsSnapshot = method.Instructions,

            Amount = request.Amount,
            Currency = PaymentCatalog.DefaultCurrency,
            ScreenshotUrl = stored.Url,
            ScreenshotPath = stored.RelativePath,
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),

            Status = PaymentStatus.Pending,
            SubmittedAt = DateTime.UtcNow,

            Purpose = PaymentPurpose.Unspecified
        };

        try
        {
            await _repository.AddAsync(payment, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            _fileService.Delete(stored.RelativePath);
            throw;
        }

        await _alerts.NotifyNewPaymentAsync(
            payment.Id, payment.Amount, payment.Currency, method.Name, cancellationToken);

        await _notifications.CreateAsync(
            userId,
            "استلام الدفع",
            "💳 تم استلام إثبات الدفع وهو الآن قيد المراجعة.",
            NotificationType.PaymentSubmitted,
            payment.Id,
            NotificationReferenceTypes.Payment,
            action: NotificationAction.Created,
            icon: "💳",
            entityName: NotificationCatalog.Payment.EntityName,
            deepLink: $"{NotificationCatalog.Payment.Route}/{payment.Id}");

        var created = await _repository.GetByIdAsync(payment.Id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("عملية الدفع مش موجودة.");

        return _mapper.Map<PaymentDetailsDto>(created);
    }

    public async Task<IReadOnlyList<PaymentMethodDto>> GetAllMethodsAsync(
        CancellationToken cancellationToken = default)
    {
        var methods = await _repository.GetMethodsAsync(activeOnly: false, cancellationToken);

        return _mapper.Map<IReadOnlyList<PaymentMethodDto>>(methods);
    }

    public async Task<PaymentMethodDto> GetMethodByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var method = await _repository.GetMethodByIdAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("طريقة الدفع مش موجودة.");

        return _mapper.Map<PaymentMethodDto>(method);
    }

    private static string DescribeMethod(PaymentMethod method) =>
        $"{method.Name} ({PaymentCatalog.GetMethodTypeName(method.Type)}) — " +
        $"{PaymentCatalog.ResolvePaymentInfo(method.Type, method.PhoneNumber, method.InstaPayIdentifier, method.AccountNumber)}" +
        $" — {(method.IsActive ? "فعال" : "غير فعال")}";

    public async Task<PaymentMethodDto> CreateMethodAsync(
        CreatePaymentMethodRequest request, CancellationToken cancellationToken = default)
    {
        var name = request.Name.Trim();

        if (await _repository.MethodNameExistsAsync(name, excludeId: null, cancellationToken))
            throw new ConflictException($"فيه طريقة دفع بالاسم ده '{name}' موجودة بالفعل.");

        var method = new PaymentMethod
        {
            Name = name,
            CreatedAt = DateTime.UtcNow
        };

        ApplyMethod(method, request);

        _repository.AddMethod(method);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.CreatePaymentMethod, AdminAuditCatalog.Targets.PaymentMethod,
            method.Id.ToString(), $"إضافة وسيلة دفع: {method.Name}",
            newValue: DescribeMethod(method), cancellationToken: cancellationToken);

        return _mapper.Map<PaymentMethodDto>(method);
    }

    public async Task<PaymentMethodDto> UpdateMethodAsync(
        int id, UpdatePaymentMethodRequest request, CancellationToken cancellationToken = default)
    {
        var method = await _repository.GetMethodByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("طريقة الدفع مش موجودة.");

        var before = DescribeMethod(method);

        var name = request.Name.Trim();

        if (await _repository.MethodNameExistsAsync(name, excludeId: id, cancellationToken))
            throw new ConflictException($"فيه طريقة دفع بالاسم ده '{name}' موجودة بالفعل.");

        method.Name = name;
        ApplyMethod(method, request);
        method.UpdatedAt = DateTime.UtcNow;

        _repository.UpdateMethod(method);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.UpdatePaymentMethod, AdminAuditCatalog.Targets.PaymentMethod,
            method.Id.ToString(), $"تعديل وسيلة دفع: {method.Name}",
            oldValue: before, newValue: DescribeMethod(method),
            cancellationToken: cancellationToken);

        return _mapper.Map<PaymentMethodDto>(method);
    }

    public async Task DeleteMethodAsync(int id, CancellationToken cancellationToken = default)
    {
        var method = await _repository.GetMethodByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("طريقة الدفع مش موجودة.");

        var usages = await _repository.CountMethodUsagesAsync(id, cancellationToken);

        if (usages > 0)
        {
            throw new ConflictException(
                $"طريقة الدفع \"{method.Name}\" مستخدمة في {usages} عملية دفع، فمش ممكن تتحذف. " +
                "أوقفها بدل ما تحذفها عشان سجل العمليات القديمة يفضل موجود.");
        }

        var removed = DescribeMethod(method);

        _repository.RemoveMethod(method);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.DeletePaymentMethod, AdminAuditCatalog.Targets.PaymentMethod,
            id.ToString(), $"حذف وسيلة دفع: {method.Name}",
            oldValue: removed, cancellationToken: cancellationToken);
    }

    private static void ApplyMethod(PaymentMethod method, CreatePaymentMethodRequest request)
    {
        method.ArabicName = Trim(request.ArabicName);
        method.Type = request.Type;
        method.Instructions = Trim(request.Instructions);
        method.IsActive = request.IsActive;
        method.DisplayOrder = request.DisplayOrder;

        method.PhoneNumber = null;
        method.InstaPayIdentifier = null;
        method.BankName = null;
        method.AccountHolderName = null;
        method.AccountNumber = null;
        method.Iban = null;

        switch (request.Type)
        {
            case PaymentMethodType.MobileWallet:
                method.PhoneNumber = Require(request.PhoneNumber, "A wallet phone number is required for a mobile wallet.");
                break;

            case PaymentMethodType.InstaPay:
                method.InstaPayIdentifier = Require(
                    request.InstaPayIdentifier, "An InstaPay address is required for an InstaPay method.");
                break;

            case PaymentMethodType.BankAccount:
                method.BankName = Require(request.BankName, "A bank name is required for a bank account.");
                method.AccountHolderName = Require(
                    request.AccountHolderName, "An account holder name is required for a bank account.");
                method.AccountNumber = Require(
                    request.AccountNumber, "An account number is required for a bank account.");
                method.Iban = Trim(request.Iban);
                break;

            case PaymentMethodType.Other:

                if (string.IsNullOrWhiteSpace(request.Instructions))
                    throw new BadRequestException("التعليمات مطلوبة لما تختار نوع «أخرى».");
                break;
        }
    }

    private static string Require(string? value, string message) =>
        string.IsNullOrWhiteSpace(value) ? throw new BadRequestException(message) : value.Trim();

    private static string? Trim(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
