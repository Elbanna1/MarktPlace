using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class UpdateBirdRequest
{
    public string SellerName { get; set; } = default!;

    public BirdType AnimalType { get; set; }

    public string? OtherType { get; set; }

    public BirdPurpose Purpose { get; set; }

    public BirdAge Age { get; set; }

    public BirdGender Gender { get; set; }

    public BirdHealthStatus HealthStatus { get; set; }

    public BirdVaccination? Vaccination { get; set; }

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

    public List<Guid> RemoveImageIds { get; set; } = new();
}
