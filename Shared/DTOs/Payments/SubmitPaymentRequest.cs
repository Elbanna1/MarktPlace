using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Payments;

public class SubmitPaymentRequest
{
    public int PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public IFormFile? Screenshot { get; set; }

    public string? Notes { get; set; }
}
