using Shared.Enums;

namespace Shared.DTOs.Payments;

public class CreatePaymentMethodRequest
{
    public string Name { get; set; } = default!;

    public string? ArabicName { get; set; }

    public PaymentMethodType Type { get; set; }

    public string? PhoneNumber { get; set; }

    public string? InstaPayIdentifier { get; set; }

    public string? BankName { get; set; }

    public string? AccountHolderName { get; set; }

    public string? AccountNumber { get; set; }

    public string? Iban { get; set; }

    public string? Instructions { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }
}

public class UpdatePaymentMethodRequest : CreatePaymentMethodRequest
{
}
