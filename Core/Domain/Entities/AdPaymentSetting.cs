namespace Domain.Entities;

public class AdPaymentSetting
{
    public int Id { get; set; } = 1;

    public bool IsEnabled { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = Shared.Constants.PaymentCatalog.DefaultCurrency;

    public string? Instructions { get; set; }

    public string? PaymentMethodIds { get; set; }

    public DateTime? EnabledAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }
}
