using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Animals;

public class HorseDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SellerName { get; set; } = default!;

    public HorseBreed Breed { get; set; }

    public string BreedName { get; set; } = default!;
    public string? OtherBreed { get; set; }

    public HorsePurpose Purpose { get; set; }

    public string PurposeName { get; set; } = default!;

    public HorseAge Age { get; set; }

    public string AgeName { get; set; } = default!;

    public HorseGender Gender { get; set; }

    public string GenderName { get; set; } = default!;

    public HorseHealthStatus HealthStatus { get; set; }

    public string HealthStatusName { get; set; } = default!;

    public HorseTrainingLevel? TrainingLevel { get; set; }

    public string TrainingLevelName { get; set; } = default!;

    public HorseVaccination? Vaccination { get; set; }

    public string VaccinationName { get; set; } = default!;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<HorseImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Horse;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
