using Shared.Constants;
using Shared.Enums;

namespace Domain.Entities;

public class BannerBooking
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public string ButtonText { get; set; } = default!;

    public string TargetUrl { get; set; } = default!;

    public bool IsInternalTarget { get; set; }

    public string DesktopImageFileName { get; set; } = default!;

    public string DesktopImagePath { get; set; } = default!;

    public string DesktopImageUrl { get; set; } = default!;

    public int DesktopImageWidth { get; set; }

    public int DesktopImageHeight { get; set; }

    public string MobileImageFileName { get; set; } = default!;

    public string MobileImagePath { get; set; } = default!;

    public string MobileImageUrl { get; set; } = default!;

    public int MobileImageWidth { get; set; }

    public int MobileImageHeight { get; set; }

    public int PlacementSettingId { get; set; }
    public BannerPlacementSetting PlacementSetting { get; set; } = default!;

    public BannerLocation Location { get; set; }

    public int SlotNumber { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }

    public int? SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int DurationDays { get; set; } = BannerBookingCatalog.DefaultDurationDays;

    public string AdvertiserName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string? WhatsAppNumber { get; set; }

    public string? Email { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = Shared.Constants.BannerBookingCatalog.DefaultCurrency;

    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = default!;

    public string? PaymentMethodNameSnapshot { get; set; }

    public string? PaymentInfoSnapshot { get; set; }

    public string? AccountNameSnapshot { get; set; }

    public string? InstructionsSnapshot { get; set; }

    public string PaymentProofUrl { get; set; } = default!;

    public string PaymentProofPath { get; set; } = default!;

    public string PaymentProofFileName { get; set; } = default!;

    public BannerPaymentStatus PaymentStatus { get; set; } = BannerPaymentStatus.Pending;

    public DateTime? PaymentApprovedAt { get; set; }

    public string? PaymentRejectionNotes { get; set; }

    public bool ConfirmationAccepted { get; set; }

    public BannerBookingStatus Status { get; set; } = BannerBookingStatus.PendingReview;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ApprovedAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? RejectedAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string? ReviewedBy { get; set; }
    public ApplicationUser? Reviewer { get; set; }

    public BannerRejectionReason? RejectionReason { get; set; }

    public string? RejectionNotes { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool OccupiesSlot =>
        Status is BannerBookingStatus.PendingReview
               or BannerBookingStatus.PaymentApproved
               or BannerBookingStatus.Approved
               or BannerBookingStatus.Published;
}
