using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class UpdateSheepGoatRequest
{
    public string SellerName { get; set; } = default!;

    public SheepGoatBreed Breed { get; set; }

    public string? OtherBreed { get; set; }

    public SheepGoatPurpose Purpose { get; set; }

    public SheepGoatAge Age { get; set; }

    public SheepGoatGender Gender { get; set; }

    public SheepGoatHealthStatus HealthStatus { get; set; }

    public SheepGoatVaccination? Vaccination { get; set; }

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
