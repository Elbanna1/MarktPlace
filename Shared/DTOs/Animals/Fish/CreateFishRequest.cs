using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class CreateFishRequest
{
    public string SellerName { get; set; } = default!;

    public FishType AnimalType { get; set; }

    public string? OtherType { get; set; }

    public FishPurpose Purpose { get; set; }

    public FishAge Age { get; set; }

    public FishHealthStatus HealthStatus { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public bool Negotiable { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public List<IFormFile> Images { get; set; } = new();

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;
}
