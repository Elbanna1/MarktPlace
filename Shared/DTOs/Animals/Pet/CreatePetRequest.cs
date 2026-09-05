using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Shared.DTOs.Animals;

public class CreatePetRequest
{
    public string SellerName { get; set; } = default!;

    public PetBreed Breed { get; set; }

    public string? OtherBreed { get; set; }

    public PetPurpose Purpose { get; set; }

    public PetAge Age { get; set; }

    public PetGender Gender { get; set; }

    public PetHealthStatus HealthStatus { get; set; }

    public PetTrainingLevel? TrainingLevel { get; set; }

    public PetVaccination? Vaccination { get; set; }

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
