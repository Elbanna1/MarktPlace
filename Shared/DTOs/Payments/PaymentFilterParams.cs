using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Payments;

public class PaymentFilterParams : PaginationParams
{
    public PaymentStatus? Status { get; set; }

    public string? UserId { get; set; }

    public int? PaymentMethodId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}
