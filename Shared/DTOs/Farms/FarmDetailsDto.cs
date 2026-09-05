using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Farms;

public class FarmDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string FarmName { get; set; } = default!;
    public FarmType FarmType { get; set; }

    public string FarmTypeName { get; set; } = default!;

    public string FarmTypeGroup { get; set; } = default!;

    public string? OtherFarmType { get; set; }

    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;
    public string? Email { get; set; }

    public decimal? AreaInFeddan { get; set; }
    public string? AvailableQuantity { get; set; }

    public AvailabilitySeason? AvailabilitySeason { get; set; }

    public string? AvailabilitySeasonName { get; set; }

    public FarmingMethod? FarmingMethod { get; set; }

    public string? FarmingMethodName { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<FarmImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Farm;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
