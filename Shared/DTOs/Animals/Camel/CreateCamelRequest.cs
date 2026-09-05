using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class CreateCamelRequest
{
    public string SellerName { get; set; } = default!;

    public CamelBreed Breed { get; set; }

    public string? OtherBreed { get; set; }

    public CamelPurpose Purpose { get; set; }

    public CamelAge Age { get; set; }

    public CamelGender Gender { get; set; }

    public CamelHealthStatus HealthStatus { get; set; }

    public CamelVaccination? Vaccination { get; set; }

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
