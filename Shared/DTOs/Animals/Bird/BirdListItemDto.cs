using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Animals;

public class BirdListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;

    public BirdType AnimalType { get; set; }
    public string AnimalTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public BirdPurpose Purpose { get; set; }
    public string PurposeName { get; set; } = default!;

    public BirdAge Age { get; set; }
    public string AgeName { get; set; } = default!;

    public BirdGender Gender { get; set; }
    public string GenderName { get; set; } = default!;

    public BirdHealthStatus HealthStatus { get; set; }
    public string HealthStatusName { get; set; } = default!;

    public BirdVaccination? Vaccination { get; set; }
    public string VaccinationName { get; set; } = default!;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<BirdImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Bird;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
