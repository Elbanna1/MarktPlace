using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.WholesaleTraders;

public class WholesaleTraderDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string TraderName { get; set; } = default!;
    public WholesaleTradeType TradeType { get; set; }

    public string TradeTypeName { get; set; } = default!;
    public string? OtherTradeType { get; set; }

    public string ProductsName { get; set; } = default!;
    public string ProductDetails { get; set; } = default!;
    public WholesaleSaleType SaleType { get; set; }

    public string SaleTypeName { get; set; } = default!;

    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;
    public string? Email { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<WholesaleTraderImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.WholesaleTrader;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
