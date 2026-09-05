using Shared.DTOs.Common;
using Shared.DTOs.Payments;
using Shared.Enums;

namespace Shared.DTOs.BannerBookings;

public class BannerImageDto
{
    public BannerImageKind Kind { get; set; }

    public string KindName { get; set; } = default!;

    public string Url { get; set; } = default!;

    public int Width { get; set; }

    public int Height { get; set; }

    public string Resolution { get; set; } = default!;
}

public class BannerPreviewDto
{
    public BannerImageDto? Desktop { get; set; }

    public BannerImageDto? Mobile { get; set; }

    public string UsageNote { get; set; } = default!;
}

public class BannerBookingSummaryDto
{
    public string Placement { get; set; } = default!;

    public BannerLocation Location { get; set; }

    public string LocationName { get; set; } = default!;

    public int SlotNumber { get; set; }

    public string? CategoryName { get; set; }

    public string? SubCategoryName { get; set; }

    public int DurationDays { get; set; }

    public string DurationDisplay { get; set; } = default!;

    public decimal Price { get; set; }

    public string Currency { get; set; } = default!;

    public string PriceDisplay { get; set; } = default!;

    public string? PaymentMethodName { get; set; }

    public bool PaymentProofUploaded { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}

public class BannerBookingListItemDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;

    public string DesktopImageUrl { get; set; } = default!;

    public string MobileImageUrl { get; set; } = default!;

    public BannerLocation Location { get; set; }

    public string LocationName { get; set; } = default!;

    public int SlotNumber { get; set; }

    public string? CategoryName { get; set; }

    public string? SubCategoryName { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = default!;

    public BannerBookingStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public BannerPaymentStatus PaymentStatus { get; set; }

    public string PaymentStatusName { get; set; } = default!;

    public DateTime SubmittedAt { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsLive { get; set; }

    public string AdvertiserName { get; set; } = default!;
}

public class BannerBookingDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public string ButtonText { get; set; } = default!;

    public string TargetUrl { get; set; } = default!;

    public bool IsInternalTarget { get; set; }

    public BannerPreviewDto Preview { get; set; } = default!;

    public BannerLocation Location { get; set; }

    public string LocationName { get; set; } = default!;

    public int SlotNumber { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public int DurationDays { get; set; }

    public string DurationDisplay { get; set; } = default!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string AdvertiserName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string? WhatsAppNumber { get; set; }

    public string? Email { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = default!;

    public string PriceDisplay { get; set; } = default!;

    public PaymentMethodSummaryDto? PaymentMethod { get; set; }

    public string PaymentProofUrl { get; set; } = default!;

    public BannerPaymentStatus PaymentStatus { get; set; }

    public string PaymentStatusName { get; set; } = default!;

    public DateTime? PaymentApprovedAt { get; set; }

    public BannerBookingStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public bool IsLive { get; set; }

    public DateTime SubmittedAt { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? RejectedAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public BannerRejectionReason? RejectionReason { get; set; }

    public string? RejectionReasonName { get; set; }

    public string? RejectionNotes { get; set; }

    public bool ConfirmationAccepted { get; set; }

    public BannerBookingSummaryDto Summary { get; set; } = default!;
}

public class PublishedBannerDto
{
    public Guid Id { get; set; }

    public BannerLocation Location { get; set; }

    public string LocationName { get; set; } = default!;

    public int SlotNumber { get; set; }

    public string SlotName { get; set; } = default!;

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public string ButtonText { get; set; } = default!;

    public string TargetUrl { get; set; } = default!;

    public bool IsInternalTarget { get; set; }

    public string DesktopImageUrl { get; set; } = default!;

    public string MobileImageUrl { get; set; } = default!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public BannerBookingStatus Status { get; set; }

    public string StatusName { get; set; } = default!;
}

public class BannerBookingFilterParams : PaginationParams
{
    public BannerBookingStatus? Status { get; set; }

    public BannerPaymentStatus? PaymentStatus { get; set; }

    public BannerLocation? Location { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string? UserId { get; set; }

    public string? Search { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}

public class BannerBookingStatusDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;
}

public class BannerRejectionReasonDto
{
    public BannerRejectionReason Value { get; set; }

    public string Name { get; set; } = default!;

    public bool RequiresNotes { get; set; }
}

public class BannerPaymentMethodDto
{
    public PaymentMethodDto Method { get; set; } = default!;

    public string? CopyLabel { get; set; }

    public string? CopyValue { get; set; }

    public string TransferNote { get; set; } = default!;
}
