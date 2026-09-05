using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Animals;

public class PetListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;

    public PetBreed Breed { get; set; }
    public string BreedName { get; set; } = default!;
    public string? OtherBreed { get; set; }

    public PetPurpose Purpose { get; set; }
    public string PurposeName { get; set; } = default!;

    public PetAge Age { get; set; }
    public string AgeName { get; set; } = default!;

    public PetGender Gender { get; set; }
    public string GenderName { get; set; } = default!;

    public PetHealthStatus HealthStatus { get; set; }
    public string HealthStatusName { get; set; } = default!;

    public PetTrainingLevel? TrainingLevel { get; set; }
    public string TrainingLevelName { get; set; } = default!;

    public PetVaccination? Vaccination { get; set; }
    public string VaccinationName { get; set; } = default!;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<PetImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Pet;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
