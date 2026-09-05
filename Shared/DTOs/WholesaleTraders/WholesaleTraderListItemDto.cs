using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.WholesaleTraders;

public class WholesaleTraderListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string TraderName { get; set; } = default!;
    public WholesaleTradeType TradeType { get; set; }

    public string TradeTypeName { get; set; } = default!;
    public string? OtherTradeType { get; set; }

    public string ProductsName { get; set; } = default!;
    public string ProductDetails { get; set; } = default!;
    public WholesaleSaleType SaleType { get; set; }

    public string SaleTypeName { get; set; } = default!;

    public string Phone { get; set; } = default!;
    public string WhatsApp { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<WholesaleTraderImageDto> Images { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.WholesaleTrader;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
