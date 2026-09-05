using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class CreateHorseRequest
{
    public string SellerName { get; set; } = default!;

    public HorseBreed Breed { get; set; }

    public string? OtherBreed { get; set; }

    public HorsePurpose Purpose { get; set; }

    public HorseAge Age { get; set; }

    public HorseGender Gender { get; set; }

    public HorseHealthStatus HealthStatus { get; set; }

    public HorseTrainingLevel? TrainingLevel { get; set; }

    public HorseVaccination? Vaccination { get; set; }

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
