using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.FruitVegetableMerchants;

public class CreateFruitVegetableMerchantRequest
{
    public string StallName { get; set; } = default!;

    public string MerchantName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string ProductName { get; set; } = default!;

    public MerchantSaleType SaleType { get; set; }

    public string ProductDetails { get; set; } = default!;

    public List<IFormFile> Images { get; set; } = new();

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}
