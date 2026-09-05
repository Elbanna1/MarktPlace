using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.FruitVegetableMerchants;

public class FruitVegetableMerchantDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string StallName { get; set; } = default!;
    public string MerchantName { get; set; } = default!;

    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string ProductName { get; set; } = default!;
    public MerchantSaleType SaleType { get; set; }

    public string SaleTypeName { get; set; } = default!;
    public string ProductDetails { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<FruitVegetableMerchantImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.FruitVegetableMerchant;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
