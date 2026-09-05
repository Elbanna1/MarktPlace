using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Animals;

public class LivestockDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SellerName { get; set; } = default!;

    public LivestockBreed Breed { get; set; }

    public string BreedName { get; set; } = default!;
    public string? OtherBreed { get; set; }

    public LivestockPurpose Purpose { get; set; }

    public string PurposeName { get; set; } = default!;

    public LivestockAge Age { get; set; }

    public string AgeName { get; set; } = default!;

    public LivestockGender Gender { get; set; }

    public string GenderName { get; set; } = default!;

    public LivestockHealthStatus HealthStatus { get; set; }

    public string HealthStatusName { get; set; } = default!;

    public LivestockVaccination? Vaccination { get; set; }

    public string VaccinationName { get; set; } = default!;

    public LivestockProduction? Production { get; set; }

    public string ProductionName { get; set; } = default!;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<LivestockImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Livestock;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
