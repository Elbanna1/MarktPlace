using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class UpdateLivestockRequest
{
    public string SellerName { get; set; } = default!;

    public LivestockBreed Breed { get; set; }

    public string? OtherBreed { get; set; }

    public LivestockPurpose Purpose { get; set; }

    public LivestockAge Age { get; set; }

    public LivestockGender Gender { get; set; }

    public LivestockHealthStatus HealthStatus { get; set; }

    public LivestockVaccination? Vaccination { get; set; }

    public LivestockProduction? Production { get; set; }

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
