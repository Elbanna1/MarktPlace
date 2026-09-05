using Shared.Enums;

namespace Shared.DTOs.Payments;

public class PaymentDetailsDto
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;

    public PaymentMethodSummaryDto PaymentMethod { get; set; } = default!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = default!;

    public string ScreenshotUrl { get; set; } = default!;

    public string? Notes { get; set; }

    public PaymentStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime SubmittedAt { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime? RejectedAt { get; set; }

    public string? RejectReason { get; set; }
}
