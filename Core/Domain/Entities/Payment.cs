using Shared.Enums;

namespace Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public int PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; } = default!;

    public string? PaymentMethodNameSnapshot { get; set; }

    public string? PaymentInfoSnapshot { get; set; }

    public string? AccountNameSnapshot { get; set; }

    public string? InstructionsSnapshot { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = Shared.Constants.PaymentCatalog.DefaultCurrency;

    public string ScreenshotUrl { get; set; } = default!;

    public string ScreenshotPath { get; set; } = default!;

    public string? Notes { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ApprovedAt { get; set; }

    public DateTime? RejectedAt { get; set; }

    public string? ApprovedBy { get; set; }
    public ApplicationUser? Reviewer { get; set; }

    public string? RejectReason { get; set; }

    public PaymentPurpose Purpose { get; set; } = PaymentPurpose.Unspecified;

    public string? TargetType { get; set; }

    public Guid? TargetId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? ActivationStartAt { get; set; }

    public DateTime? ActivationEndAt { get; set; }

    public DateTime? ActivatedAt { get; set; }
}
