namespace Shared.DTOs.FruitVegetableMerchants;

public class FruitVegetableMerchantImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
