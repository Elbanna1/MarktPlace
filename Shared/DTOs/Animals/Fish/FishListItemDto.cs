using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Animals;

public class FishListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;

    public FishType AnimalType { get; set; }
    public string AnimalTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public FishPurpose Purpose { get; set; }
    public string PurposeName { get; set; } = default!;

    public FishAge Age { get; set; }
    public string AgeName { get; set; } = default!;

    public FishHealthStatus HealthStatus { get; set; }
    public string HealthStatusName { get; set; } = default!;

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<FishImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Fish;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
