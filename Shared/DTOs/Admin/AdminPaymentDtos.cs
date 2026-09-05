using System.ComponentModel.DataAnnotations;
using Shared.Constants;
using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Admin;

public class AdminPaymentFilterParams : PaginationParams
{
    public string? Search { get; set; }

    public PaymentStatus? Status { get; set; }

    public int? PaymentMethodId { get; set; }

    public string? UserId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}

public class AdminPaymentListItemDto
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;

    public string? UserName { get; set; }

    public string? UserPhone { get; set; }

    public string? UserEmail { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = default!;

    public int PaymentMethodId { get; set; }

    public string PaymentMethodName { get; set; } = default!;

    public string ScreenshotUrl { get; set; } = default!;

    public PaymentStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }
}

public class AdminPaymentDetailsDto : AdminPaymentListItemDto
{
    public string? Notes { get; set; }

    public AdminPaymentSnapshotDto Snapshot { get; set; } = new();

    public string? CurrentPaymentMethodName { get; set; }

    public string? ReviewedBy { get; set; }

    public string? ReviewedByName { get; set; }

    public string? RejectReason { get; set; }

    public AdminDashboardBannerRequestDto? BannerRequest { get; set; }
}

public class AdminPaymentSnapshotDto
{
    public string? PaymentMethodName { get; set; }

    public string? PaymentInfo { get; set; }

    public string? AccountName { get; set; }

    public string? Instructions { get; set; }

    public bool IsSnapshot { get; set; }
}

public class RejectPaymentRequest
{
    [Required(ErrorMessage = "سبب الرفض مطلوب.")]
    [MaxLength(PaymentCatalog.MaxRejectReasonLength,
        ErrorMessage = "لا يمكن أن يتجاوز سبب الرفض 500 حرف.")]
    public string Reason { get; set; } = default!;
}

public class RefundPaymentRequest
{
    [Required(ErrorMessage = "سبب الاسترداد مطلوب.")]
    [MaxLength(PaymentCatalog.MaxRejectReasonLength,
        ErrorMessage = "لا يمكن أن يتجاوز السبب 500 حرف.")]
    public string Reason { get; set; } = default!;
}
