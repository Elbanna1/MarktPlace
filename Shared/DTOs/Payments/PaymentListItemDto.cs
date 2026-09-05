using Shared.Enums;

namespace Shared.DTOs.Payments;

public class PaymentListItemDto
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = default!;

    public PaymentMethodSummaryDto PaymentMethod { get; set; } = default!;

    public string ScreenshotUrl { get; set; } = default!;

    public PaymentStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime SubmittedAt { get; set; }

    public string? RejectReason { get; set; }
}
