using AutoMapper;
using Domain.Entities;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Advertisements;
using Shared.DTOs.BannerBookings;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Responses;

namespace Services.BannerBookings;

public class BannerBookingService : IBannerBookingService
{
    private readonly IBannerBookingRepository _repository;
    private readonly IPaymentService _paymentService;
    private readonly IFileService _fileService;
    private readonly INotificationService _notifications;
    private readonly IMapper _mapper;
    private readonly IAdminAuditService _audit;
    private readonly IAdminAlertService _alerts;

    public BannerBookingService(
        IBannerBookingRepository repository,
        IPaymentService paymentService,
        IFileService fileService,
        INotificationService notifications,
        IMapper mapper,
        IAdminAuditService audit,
        IAdminAlertService alerts)
    {
        _repository = repository;
        _paymentService = paymentService;
        _fileService = fileService;
        _notifications = notifications;
        _mapper = mapper;
        _audit = audit;
        _alerts = alerts;
    }

    public async Task<IReadOnlyList<BannerPlacementDto>> GetPlacementsAsync(
        CancellationToken cancellationToken = default)
    {
        var placements = await _repository.GetPlacementsAsync(activeOnly: true, cancellationToken);

        return placements.Select(BannerPlacementPresenter.ToDto).ToList();
    }

    public async Task<BannerAvailabilityDto> GetAvailabilityAsync(
        BannerAvailabilityQuery query, CancellationToken cancellationToken = default)
    {
        var placement = await GetActivePlacementAsync(query.Location, cancellationToken);
        var scope = await ResolveScopeAsync(query.Location, query.CategoryId, query.SubCategoryId, cancellationToken);

        var (startDate, endDate) = NextWindow(placement.DurationDays);

        var slotCount = SlotCount(placement);
        var slots = new List<BannerSlotAvailabilityDto>(slotCount);

        for (var slotNumber = 1; slotNumber <= slotCount; slotNumber++)
        {
            var occupying = await _repository.GetOverlappingAsync(
                placement.Location, slotNumber, scope.CategoryId, scope.SubCategoryId,
                startDate, endDate, excludeBookingId: null, cancellationToken);

            var isAvailable = occupying.Count == 0;

            slots.Add(new BannerSlotAvailabilityDto
            {
                SlotNumber = slotNumber,
                SlotName = BannerBookingCatalog.GetSlotName(slotNumber),
                IsAvailable = isAvailable,
                StatusName = isAvailable ? "متاح" : "محجوز",

                ReservedUntil = isAvailable ? null : occupying.Max(booking => booking.EndDate),
                AvailableFrom = isAvailable ? null : occupying.Max(booking => booking.EndDate)
            });
        }

        var availableSlots = slots.Where(slot => slot.IsAvailable).Select(slot => slot.SlotNumber).ToList();
        var anyAvailable = availableSlots.Count > 0;

        return new BannerAvailabilityDto
        {
            Location = placement.Location,
            LocationName = BannerBookingCatalog.GetLocationName(placement.Location),
            Placement = BannerPlacementPresenter.ToDto(placement),

            Slots = slots,
            AvailableSlots = availableSlots,
            IsAvailable = anyAvailable,
            Message = anyAvailable ? BannerBookingCatalog.AvailableMessage : BannerBookingCatalog.UnavailableMessage,

            CategoryId = scope.CategoryId,
            CategoryName = scope.CategoryName,
            SubCategoryId = scope.SubCategoryId,
            SubCategoryName = scope.SubCategoryName,

            NextStartDate = anyAvailable ? startDate : null,
            NextEndDate = anyAvailable ? endDate : null
        };
    }

    public async Task<IReadOnlyList<BannerPaymentMethodDto>> GetPaymentMethodsAsync(
        CancellationToken cancellationToken = default)
    {
        var methods = await _paymentService.GetMethodsAsync(cancellationToken);

        return methods.Select(method => new BannerPaymentMethodDto
        {
            Method = method,
            CopyLabel = CopyLabel(method.Type),
            CopyValue = CopyValue(method),
            TransferNote = BannerBookingCatalog.PaymentTransferNote
        }).ToList();
    }

    public async Task<BannerBookingSummaryDto> GetQuoteAsync(
        BannerBookingQuoteQuery query, CancellationToken cancellationToken = default)
    {
        var placement = await GetActivePlacementAsync(query.Location, cancellationToken);
        var scope = await ResolveScopeAsync(query.Location, query.CategoryId, query.SubCategoryId, cancellationToken);

        var slotNumber = ResolveSlotNumber(placement, query.SlotNumber);
        var (startDate, endDate) = NextWindow(placement.DurationDays);

        await EnsureSlotIsFreeAsync(placement, slotNumber, scope, startDate, endDate, null, cancellationToken);

        string? paymentMethodName = null;
        if (query.PaymentMethodId is { } methodId)
        {
            var method = await _repository.GetPaymentMethodAsync(methodId, cancellationToken)
                ?? throw new NotFoundException("طريقة الدفع غير موجودة.");

            paymentMethodName = method.ArabicName ?? method.Name;
        }

        return new BannerBookingSummaryDto
        {
            Placement = BannerPlacementPresenter.FormatPlacement(
                placement.Location, slotNumber, scope.CategoryName, scope.SubCategoryName),
            Location = placement.Location,
            LocationName = BannerBookingCatalog.GetLocationName(placement.Location),
            SlotNumber = slotNumber,
            CategoryName = scope.CategoryName,
            SubCategoryName = scope.SubCategoryName,

            DurationDays = placement.DurationDays,
            DurationDisplay = BannerBookingCatalog.FormatDuration(placement.DurationDays),

            Price = placement.Price,
            Currency = BannerBookingCatalog.DefaultCurrency,
            PriceDisplay = BannerPlacementPresenter.FormatPrice(placement.Price),

            PaymentMethodName = paymentMethodName,

            PaymentProofUploaded = false,

            StartDate = startDate,
            EndDate = endDate
        };
    }

    public async Task<BannerPreviewDto> PreviewImagesAsync(
        BannerLocation location,
        UploadImageModel? desktopImage,
        UploadImageModel? mobileImage,
        CancellationToken cancellationToken = default)
    {
        if (desktopImage is null && mobileImage is null)
            throw new BadRequestException("ارفع صورة Desktop أو Mobile للمعاينة.");

        var placement = await GetActivePlacementAsync(location, cancellationToken);

        var preview = new BannerPreviewDto { UsageNote = BannerBookingCatalog.ImageUsageNote };
        var written = new List<string>();

        try
        {
            if (desktopImage is not null)
                preview.Desktop = await StorePreviewAsync(
                    desktopImage, BannerImageRules.DesktopRequirement(placement), written, cancellationToken);

            if (mobileImage is not null)
                preview.Mobile = await StorePreviewAsync(
                    mobileImage, BannerImageRules.MobileRequirement(placement), written, cancellationToken);
        }
        catch
        {
            foreach (var path in written)
                _fileService.Delete(path);

            throw;
        }

        return preview;
    }

    public IReadOnlyList<BannerRejectionReasonDto> GetRejectionReasons() =>
        Enum.GetValues<BannerRejectionReason>()
            .Select(reason => new BannerRejectionReasonDto
            {
                Value = reason,
                Name = BannerBookingCatalog.GetRejectionReasonName(reason),

                RequiresNotes = reason == BannerRejectionReason.Other
            })
            .ToList();

    public IReadOnlyList<BannerBookingStatusDto> GetStatuses() =>
        Enum.GetValues<BannerBookingStatus>()
            .Select(status => new BannerBookingStatusDto
            {
                Id = (int)status,
                Name = status.ToString(),
                NameAr = BannerBookingCatalog.GetStatusName(status)
            })
            .ToList();

    public async Task<BannerBookingDto> CreateAsync(
        string userId,
        CreateBannerBookingRequest request,
        UploadImageModel? desktopImage,
        UploadImageModel? mobileImage,
        UploadImageModel? paymentProof,
        CancellationToken cancellationToken = default)
    {
        if (!request.ConfirmationAccepted)
            throw new BadRequestException(BannerBookingCatalog.ConfirmationStatement);

        var placement = await GetActivePlacementAsync(request.Location, cancellationToken);
        var scope = await ResolveScopeAsync(
            request.Location, request.CategoryId, request.SubCategoryId, cancellationToken);

        var slotNumber = ResolveSlotNumber(placement, request.SlotNumber);
        var target = BannerTargetUrl.Parse(request.TargetUrl);

        var method = await _repository.GetPaymentMethodAsync(request.PaymentMethodId, cancellationToken)
            ?? throw new NotFoundException("طريقة الدفع غير موجودة.");

        if (!method.IsActive)
            throw new BadRequestException($"طريقة الدفع '{method.ArabicName ?? method.Name}' لم تعد متاحة.");

        if (paymentProof is null)
            throw new BadRequestException("صورة إثبات الدفع مطلوبة.");

        var desktopDimensions = BannerImageRules.Validate(
            desktopImage, BannerImageRules.DesktopRequirement(placement));

        var mobileDimensions = BannerImageRules.Validate(
            mobileImage, BannerImageRules.MobileRequirement(placement));

        var (startDate, endDate) = NextWindow(placement.DurationDays);

        await EnsureSlotIsFreeAsync(placement, slotNumber, scope, startDate, endDate, null, cancellationToken);

        var stored = new List<string>();
        BannerBooking booking;

        try
        {
            var desktopFile = await _fileService.SaveAsync(
                desktopImage!, ImageConstants.BannerBookingsFolder, cancellationToken);
            stored.Add(desktopFile.RelativePath);

            var mobileFile = await _fileService.SaveAsync(
                mobileImage!, ImageConstants.BannerBookingsFolder, cancellationToken);
            stored.Add(mobileFile.RelativePath);

            var proofFile = await _fileService.SaveAsync(
                paymentProof, ImageConstants.BannerBookingProofFolder, cancellationToken);
            stored.Add(proofFile.RelativePath);

            booking = new BannerBooking
            {
                Id = Guid.NewGuid(),
                UserId = userId,

                Title = request.Title.Trim(),
                Description = Trim(request.Description),
                ButtonText = request.ButtonText.Trim(),
                TargetUrl = target.Url,
                IsInternalTarget = target.IsInternal,

                DesktopImageFileName = desktopFile.FileName,
                DesktopImagePath = desktopFile.RelativePath,
                DesktopImageUrl = desktopFile.Url,
                DesktopImageWidth = desktopDimensions.Width,
                DesktopImageHeight = desktopDimensions.Height,

                MobileImageFileName = mobileFile.FileName,
                MobileImagePath = mobileFile.RelativePath,
                MobileImageUrl = mobileFile.Url,
                MobileImageWidth = mobileDimensions.Width,
                MobileImageHeight = mobileDimensions.Height,

                PlacementSettingId = placement.Id,
                Location = placement.Location,
                SlotNumber = slotNumber,
                CategoryId = scope.CategoryId,
                SubCategoryId = scope.SubCategoryId,

                StartDate = startDate,
                EndDate = endDate,

                DurationDays = BannerBookingCatalog.NormalizeDuration(placement.DurationDays),

                AdvertiserName = request.AdvertiserName.Trim(),
                PhoneNumber = request.PhoneNumber.Trim(),
                WhatsAppNumber = Trim(request.WhatsAppNumber),
                Email = Trim(request.Email),

                Price = placement.Price,
                Currency = BannerBookingCatalog.DefaultCurrency,

                PaymentMethodId = method.Id,

                PaymentMethodNameSnapshot = method.Name,
                PaymentInfoSnapshot = PaymentCatalog.ResolvePaymentInfo(
                    method.Type, method.PhoneNumber, method.InstaPayIdentifier, method.AccountNumber),
                AccountNameSnapshot = method.AccountHolderName,
                InstructionsSnapshot = method.Instructions,

                PaymentProofUrl = proofFile.Url,
                PaymentProofPath = proofFile.RelativePath,
                PaymentProofFileName = proofFile.FileName,
                PaymentStatus = BannerPaymentStatus.Pending,

                ConfirmationAccepted = true,
                Status = BannerBookingStatus.PendingReview,
                SubmittedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(booking, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            foreach (var path in stored)
                _fileService.Delete(path);

            throw;
        }

        await NotifyAsync(
            userId, NotificationCatalog.BannerBookings.Submitted(booking.Id, booking.Title),
            NotificationAction.Created, booking.Id);

        await _alerts.NotifyNewBannerRequestAsync(
            booking.Id, booking.Title, booking.AdvertiserName, booking.Price, cancellationToken);

        return await ReadAsync(booking.Id, cancellationToken);
    }

    public async Task<IReadOnlyList<BannerBookingListItemDto>> GetMyBookingsAsync(
        string userId, BannerBookingStatus? status = null, CancellationToken cancellationToken = default)
    {
        var bookings = await _repository.GetByUserAsync(userId, status, cancellationToken);

        return ToListItems(bookings, DateTime.UtcNow);
    }

    public async Task<BannerBookingDto> GetByIdAsync(
        Guid id, string userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("حجز الإعلان غير موجود.");

        if (!isAdmin && !string.Equals(booking.UserId, userId, StringComparison.Ordinal))
            throw new NotFoundException("حجز الإعلان غير موجود.");

        return ToDto(booking, DateTime.UtcNow);
    }

    public async Task<BannerBookingDto> CancelAsync(
        Guid id, string userId, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("حجز الإعلان غير موجود.");

        if (!string.Equals(booking.UserId, userId, StringComparison.Ordinal))
            throw new NotFoundException("حجز الإعلان غير موجود.");

        if (booking.Status is not (BannerBookingStatus.PendingReview or BannerBookingStatus.PaymentApproved))
            throw new BadRequestException("لا يمكن إلغاء الحجز بعد الموافقة عليه.");

        booking.Status = BannerBookingStatus.Cancelled;
        booking.CancelledAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;

        _repository.Update(booking);
        await _repository.SaveChangesAsync(cancellationToken);

        return ToDto(booking, DateTime.UtcNow);
    }

    public async Task<IReadOnlyList<PublishedBannerDto>> GetPublishedAsync(
        BannerLocation? location = null,
        int? categoryId = null,
        int? subCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var bookings = await _repository.GetPublishedAsync(
            DateTime.UtcNow, location, categoryId, subCategoryId, cancellationToken);

        return _mapper.Map<IReadOnlyList<PublishedBannerDto>>(bookings);
    }

    public async Task<IReadOnlyList<PublishedBannerDto>> GetPublishedSliderAsync(
        BannerLocation location, CancellationToken cancellationToken = default)
    {
        if (location is not (BannerLocation.HomeSlider1 or BannerLocation.HomeSlider2))
            throw new BadRequestException("هذا المكان الإعلاني ليس سلايدر في الصفحة الرئيسية.");

        return await GetPublishedAsync(location, categoryId: null, subCategoryId: null, cancellationToken);
    }

    public async Task<IReadOnlyList<PublishedBannerDto>> GetPublishedSubCategoryAsync(
        int categoryId, int subCategoryId, CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(
            BannerLocation.SubCategoryBanner, categoryId, subCategoryId, cancellationToken);

        return await GetPublishedAsync(
            BannerLocation.SubCategoryBanner, scope.CategoryId, scope.SubCategoryId, cancellationToken);
    }

    public async Task<PaginatedResult<BannerBookingListItemDto>> GetAllAsync(
        BannerBookingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(filter, cancellationToken);

        return new PaginatedResult<BannerBookingListItemDto>(
            ToListItems(items, DateTime.UtcNow), totalCount, filter.PageIndex, filter.PageSize);
    }

    public async Task<BannerBookingDto> ApprovePaymentAsync(
        Guid id, string adminId, CancellationToken cancellationToken = default)
    {
        var booking = await GetForReviewAsync(id, cancellationToken);

        if (booking.PaymentStatus == BannerPaymentStatus.Paid)
            throw new BadRequestException("تم تأكيد الدفع بالفعل.");

        if (booking.Status is BannerBookingStatus.Rejected or BannerBookingStatus.Cancelled
                           or BannerBookingStatus.Expired)
        {
            throw new BadRequestException("لا يمكن تأكيد الدفع لحجز منتهٍ أو مرفوض أو ملغي.");
        }

        var now = DateTime.UtcNow;

        var previousPaymentStatus = booking.PaymentStatus;

        booking.PaymentStatus = BannerPaymentStatus.Paid;
        booking.PaymentApprovedAt = now;
        booking.PaymentRejectionNotes = null;
        booking.ReviewedBy = adminId;
        booking.UpdatedAt = now;

        if (booking.Status == BannerBookingStatus.PendingReview)
            booking.Status = BannerBookingStatus.PaymentApproved;

        _repository.Update(booking);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.ApproveBannerPayment, AdminAuditCatalog.Targets.BannerBooking,
            id.ToString(), $"تأكيد دفع بانر: {booking.Title}",
            oldValue: BannerBookingCatalog.GetPaymentStatusName(previousPaymentStatus),
            newValue: BannerBookingCatalog.GetPaymentStatusName(BannerPaymentStatus.Paid),
            adminUserId: adminId, cancellationToken: cancellationToken);

        await NotifyAsync(
            booking.UserId, NotificationCatalog.BannerBookings.PaymentApproved(booking.Id, booking.Title),
            NotificationAction.Approved, booking.Id);

        return ToDto(booking, now);
    }

    public async Task<BannerBookingDto> RejectPaymentAsync(
        Guid id, string adminId, RejectBannerPaymentRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await GetForReviewAsync(id, cancellationToken);

        if (booking.Status is BannerBookingStatus.Published or BannerBookingStatus.Approved)
            throw new BadRequestException("لا يمكن رفض الدفع بعد الموافقة على الإعلان.");

        var now = DateTime.UtcNow;

        var previousPaymentStatus = booking.PaymentStatus;

        booking.PaymentStatus = BannerPaymentStatus.Rejected;
        booking.PaymentApprovedAt = null;
        booking.PaymentRejectionNotes = Trim(request.Notes);
        booking.ReviewedBy = adminId;
        booking.UpdatedAt = now;

        booking.Status = BannerBookingStatus.PendingReview;

        _repository.Update(booking);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.RejectBannerPayment, AdminAuditCatalog.Targets.BannerBooking,
            id.ToString(), $"رفض دفع بانر: {booking.Title}",
            oldValue: BannerBookingCatalog.GetPaymentStatusName(previousPaymentStatus),
            newValue: BannerBookingCatalog.GetPaymentStatusName(BannerPaymentStatus.Rejected),
            adminUserId: adminId, cancellationToken: cancellationToken);

        await NotifyAsync(
            booking.UserId,
            NotificationCatalog.BannerBookings.PaymentRejected(booking.Id, booking.Title, booking.PaymentRejectionNotes),
            NotificationAction.Rejected, booking.Id);

        return ToDto(booking, now);
    }

    public async Task<BannerBookingDto> ApproveAsync(
        Guid id, string adminId, CancellationToken cancellationToken = default)
    {
        var booking = await GetForReviewAsync(id, cancellationToken);

        if (booking.Status is BannerBookingStatus.Approved or BannerBookingStatus.Published)
            throw new BadRequestException("تمت الموافقة على هذا الحجز بالفعل.");

        if (booking.Status is BannerBookingStatus.Rejected or BannerBookingStatus.Cancelled
                           or BannerBookingStatus.Expired)
        {
            throw new BadRequestException("لا يمكن الموافقة على حجز منتهٍ أو مرفوض أو ملغي.");
        }

        if (booking.PaymentStatus != BannerPaymentStatus.Paid)
            throw new BadRequestException("يجب تأكيد الدفع قبل الموافقة على الإعلان.");

        var now = DateTime.UtcNow;
        var previousStatus = booking.Status;

        var (startDate, endDate) = NextWindow(booking.DurationDays);

        if (startDate > booking.StartDate)
        {
            var scope = new PlacementScope(
                booking.CategoryId, booking.SubCategoryId, booking.Category?.NameAr, booking.SubCategory?.NameAr);

            var placement = await _repository.GetPlacementAsync(booking.Location, asNoTracking: true, cancellationToken)
                ?? throw new NotFoundException("لا توجد إعدادات لهذا المكان الإعلاني.");

            await EnsureSlotIsFreeAsync(
                placement, booking.SlotNumber, scope, startDate, endDate, booking.Id, cancellationToken);

            booking.StartDate = startDate;
            booking.EndDate = endDate;
        }

        booking.ApprovedAt = now;
        booking.ReviewedBy = adminId;
        booking.RejectionReason = null;
        booking.RejectionNotes = null;
        booking.UpdatedAt = now;

        var goesLiveNow = booking.StartDate <= now;

        booking.Status = goesLiveNow ? BannerBookingStatus.Published : BannerBookingStatus.Approved;
        booking.PublishedAt = goesLiveNow ? now : null;

        _repository.Update(booking);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.ApproveBanner, AdminAuditCatalog.Targets.BannerBooking,
            id.ToString(), $"الموافقة على بانر: {booking.Title}",
            oldValue: BannerBookingCatalog.GetStatusName(previousStatus),
            newValue: BannerBookingCatalog.GetStatusName(booking.Status),
            adminUserId: adminId, cancellationToken: cancellationToken);

        var content = goesLiveNow
            ? NotificationCatalog.BannerBookings.Published(
                booking.Id, booking.Title,
                BannerPlacementPresenter.FormatPlacement(
                    booking.Location, booking.SlotNumber, booking.Category?.NameAr, booking.SubCategory?.NameAr),
                booking.EndDate)
            : NotificationCatalog.BannerBookings.Approved(booking.Id, booking.Title, booking.StartDate);

        await NotifyAsync(booking.UserId, content, NotificationAction.Approved, booking.Id);

        return ToDto(booking, now);
    }

    public async Task<BannerBookingDto> RejectAsync(
        Guid id, string adminId, RejectBannerBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await GetForReviewAsync(id, cancellationToken);

        if (booking.Status == BannerBookingStatus.Rejected)
            throw new BadRequestException("تم رفض هذا الحجز بالفعل.");

        if (booking.Status == BannerBookingStatus.Cancelled)
            throw new BadRequestException("تم إلغاء هذا الحجز من قبل المعلن.");

        if (request.Reason == BannerRejectionReason.Other && string.IsNullOrWhiteSpace(request.Notes))
            throw new BadRequestException("يجب كتابة سبب الرفض عند اختيار 'سبب آخر'.");

        var previousStatus = booking.Status;

        var now = DateTime.UtcNow;

        booking.Status = BannerBookingStatus.Rejected;
        booking.RejectionReason = request.Reason;
        booking.RejectionNotes = Trim(request.Notes);
        booking.RejectedAt = now;
        booking.ReviewedBy = adminId;
        booking.PublishedAt = null;
        booking.UpdatedAt = now;

        _repository.Update(booking);

        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.RejectBanner, AdminAuditCatalog.Targets.BannerBooking,
            id.ToString(),
            $"رفض بانر: {booking.Title} — {BannerBookingCatalog.GetRejectionReasonName(request.Reason)}",
            oldValue: BannerBookingCatalog.GetStatusName(previousStatus),
            newValue: BannerBookingCatalog.GetStatusName(BannerBookingStatus.Rejected),
            adminUserId: adminId, cancellationToken: cancellationToken);

        await NotifyAsync(
            booking.UserId,
            NotificationCatalog.BannerBookings.Rejected(
                booking.Id, booking.Title,
                BannerBookingCatalog.GetRejectionReasonName(request.Reason), booking.RejectionNotes),
            NotificationAction.Rejected, booking.Id);

        return ToDto(booking, now);
    }

    public async Task<BannerBookingDto> ExpireAsync(
        Guid id, string adminId, CancellationToken cancellationToken = default)
    {
        var booking = await GetForReviewAsync(id, cancellationToken);

        if (booking.Status is not (BannerBookingStatus.Published or BannerBookingStatus.Approved))
            throw new BadRequestException("لا يمكن إنهاء حجز غير منشور.");

        var previousStatus = booking.Status;

        var now = DateTime.UtcNow;

        booking.Status = BannerBookingStatus.Expired;
        booking.ExpiredAt = now;
        booking.EndDate = now;
        booking.ReviewedBy = adminId;
        booking.UpdatedAt = now;

        _repository.Update(booking);
        await _repository.SaveChangesAsync(cancellationToken);

        await _audit.LogAsync(
            AdminAuditAction.ExpireBanner, AdminAuditCatalog.Targets.BannerBooking,
            id.ToString(), $"إنهاء بانر: {booking.Title}",
            oldValue: BannerBookingCatalog.GetStatusName(previousStatus),
            newValue: BannerBookingCatalog.GetStatusName(BannerBookingStatus.Expired),
            adminUserId: adminId, cancellationToken: cancellationToken);

        await NotifyAsync(
            booking.UserId, NotificationCatalog.BannerBookings.Expired(booking.Id, booking.Title, booking.DurationDays),
            NotificationAction.Expired, booking.Id);

        return ToDto(booking, now);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var booking = await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("حجز الإعلان غير موجود.");

        var paths = new[] { booking.DesktopImagePath, booking.MobileImagePath, booking.PaymentProofPath };

        _repository.Remove(booking);
        await _repository.SaveChangesAsync(cancellationToken);

        foreach (var path in paths)
            _fileService.Delete(path);
    }

    private readonly record struct PlacementScope(
        int? CategoryId, int? SubCategoryId, string? CategoryName, string? SubCategoryName);

    private static (DateTime StartDate, DateTime EndDate) NextWindow(int durationDays)
    {
        var start = DateTime.UtcNow.Date;
        return (start, start.AddDays(BannerBookingCatalog.NormalizeDuration(durationDays)));
    }

    private async Task<BannerPlacementSetting> GetActivePlacementAsync(
        BannerLocation location, CancellationToken cancellationToken)
    {
        var placement = await _repository.GetPlacementAsync(location, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("لا توجد إعدادات لهذا المكان الإعلاني.");

        if (!placement.IsActive)
            throw new BadRequestException($"{BannerBookingCatalog.GetLocationName(location)} غير متاح للحجز حاليًا.");

        return placement;
    }

    private async Task<PlacementScope> ResolveScopeAsync(
        BannerLocation location, int? categoryId, int? subCategoryId, CancellationToken cancellationToken)
    {
        if (location != BannerLocation.SubCategoryBanner)
            return new PlacementScope(null, null, null, null);

        if (categoryId is null || subCategoryId is null)
            throw new BadRequestException("يجب اختيار القسم والقسم الفرعي لبانر القسم الفرعي.");

        var subCategory = await _repository.GetSubCategoryAsync(subCategoryId.Value, cancellationToken)
            ?? throw new NotFoundException("القسم الفرعي غير موجود.");

        if (subCategory.CategoryId != categoryId.Value)
            throw new BadRequestException("القسم الفرعي لا ينتمي إلى القسم المختار.");

        return new PlacementScope(
            subCategory.CategoryId, subCategory.Id, subCategory.Category?.NameAr, subCategory.NameAr);
    }

    private static int SlotCount(BannerPlacementSetting placement) =>
        placement.Location == BannerLocation.SubCategoryBanner
            ? 1
            : Math.Max(1, placement.MaxSlots);

    private static int ResolveSlotNumber(BannerPlacementSetting placement, int? requested)
    {
        var count = SlotCount(placement);

        if (count == 1)
            return 1;

        if (requested is null)
            throw new BadRequestException("يجب اختيار مكان الإعلان (Slot).");

        if (requested < 1 || requested > count)
            throw new BadRequestException($"رقم المكان غير صحيح. الأماكن المتاحة من 1 إلى {count}.");

        return requested.Value;
    }

    private async Task EnsureSlotIsFreeAsync(
        BannerPlacementSetting placement,
        int slotNumber,
        PlacementScope scope,
        DateTime startDate,
        DateTime endDate,
        Guid? excludeBookingId,
        CancellationToken cancellationToken)
    {
        var overlapping = await _repository.GetOverlappingAsync(
            placement.Location, slotNumber, scope.CategoryId, scope.SubCategoryId,
            startDate, endDate, excludeBookingId, cancellationToken);

        if (overlapping.Count == 0)
            return;

        var freeFrom = overlapping.Max(booking => booking.EndDate);

        throw new ConflictException(
            $"{BannerBookingCatalog.UnavailableMessage} " +
            $"({BannerPlacementPresenter.FormatPlacement(placement.Location, slotNumber, scope.CategoryName, scope.SubCategoryName)}) " +
            $"— المساحة متاحة اعتبارًا من {freeFrom:yyyy/MM/dd}.");
    }

    private async Task<BannerImageDto> StorePreviewAsync(
        UploadImageModel image,
        BannerImageRules.Requirement requirement,
        List<string> written,
        CancellationToken cancellationToken)
    {
        var dimensions = BannerImageRules.Validate(image, requirement);

        var stored = await _fileService.SaveAsync(
            image, ImageConstants.BannerBookingPreviewFolder, cancellationToken);

        written.Add(stored.RelativePath);

        return new BannerImageDto
        {
            Kind = requirement.Kind,
            KindName = BannerBookingCatalog.GetImageKindName(requirement.Kind),
            Url = stored.Url,
            Width = dimensions.Width,
            Height = dimensions.Height,
            Resolution = dimensions.ToString()
        };
    }

    private async Task<BannerBooking> GetForReviewAsync(Guid id, CancellationToken cancellationToken) =>
        await _repository.GetByIdAsync(id, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException("حجز الإعلان غير موجود.");

    private async Task<BannerBookingDto> ReadAsync(Guid id, CancellationToken cancellationToken)
    {
        var booking = await _repository.GetByIdAsync(id, asNoTracking: true, cancellationToken)
            ?? throw new NotFoundException("حجز الإعلان غير موجود.");

        return ToDto(booking, DateTime.UtcNow);
    }

    private BannerBookingDto ToDto(BannerBooking booking, DateTime utcNow)
    {
        var dto = _mapper.Map<BannerBookingDto>(booking);
        dto.IsLive = IsLive(booking, utcNow);

        return dto;
    }

    private IReadOnlyList<BannerBookingListItemDto> ToListItems(
        IReadOnlyList<BannerBooking> bookings, DateTime utcNow)
    {
        var items = _mapper.Map<List<BannerBookingListItemDto>>(bookings);

        for (var index = 0; index < items.Count; index++)
            items[index].IsLive = IsLive(bookings[index], utcNow);

        return items;
    }

    private static bool IsLive(BannerBooking booking, DateTime utcNow) =>
        booking.Status == BannerBookingStatus.Published &&
        booking.StartDate <= utcNow &&
        booking.EndDate > utcNow;

    private Task NotifyAsync(
        string userId, NotificationContent content, NotificationAction action, Guid bookingId) =>
        _notifications.CreateAsync(
            userId,
            content.Title,
            content.Message,
            content.Type,
            bookingId,
            content.ReferenceType,
            action,
            content.Icon,
            content.EntityName,
            content.DeepLink);

    private static string? CopyLabel(PaymentMethodType type) => type switch
    {
        PaymentMethodType.MobileWallet => "رقم المحفظة",
        PaymentMethodType.InstaPay => "عنوان / رقم InstaPay",
        PaymentMethodType.BankAccount => "رقم الحساب",
        _ => null
    };

    private static string? CopyValue(Shared.DTOs.Payments.PaymentMethodDto method) => method.Type switch
    {
        PaymentMethodType.MobileWallet => method.PhoneNumber,
        PaymentMethodType.InstaPay => method.InstaPayId,
        PaymentMethodType.BankAccount => method.Bank?.AccountNumber,
        _ => null
    };

    private static string? Trim(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
