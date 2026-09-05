using Shared.Enums;

namespace Shared.DTOs.Payments;

public class PaymentMethodDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string? ArabicName { get; set; }

    public PaymentMethodType Type { get; set; }

    public string TypeName { get; set; } = default!;

    public string? PhoneNumber { get; set; }

    public string? InstaPayId { get; set; }

    public BankAccountDto? Bank { get; set; }

    public string? Instructions { get; set; }

    public bool IsActive { get; set; }

    public int DisplayOrder { get; set; }
}

public class BankAccountDto
{
    public string? BankName { get; set; }

    public string? AccountHolderName { get; set; }

    public string? AccountNumber { get; set; }

    public string? Iban { get; set; }
}

public class PaymentMethodSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? ArabicName { get; set; }
    public PaymentMethodType Type { get; set; }
    public string TypeName { get; set; } = default!;
}
