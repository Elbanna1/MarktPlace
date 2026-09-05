using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Antiques;

public class HandmadeImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class HandmadeVideoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
}

public class HandmadeColorDto
{
    public HandmadeColor Color { get; set; }

    public string Name { get; set; } = default!;
}

public class HandmadeDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string ProductName { get; set; } = default!;

    public HandmadeType HandmadeType { get; set; }

    public string HandmadeTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public string Material { get; set; } = default!;

    public bool IsFullyHandmade { get; set; }
    public bool CustomOrder { get; set; }
    public string? ProductionTime { get; set; }
    public string? Size { get; set; }

    public List<HandmadeColorDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<HandmadeImageDto> Images { get; set; } = new();
    public HandmadeVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Handmade;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class HandmadeListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string ProductName { get; set; } = default!;

    public HandmadeType HandmadeType { get; set; }
    public string HandmadeTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public string Material { get; set; } = default!;

    public bool IsFullyHandmade { get; set; }
    public bool CustomOrder { get; set; }
    public string? ProductionTime { get; set; }
    public string? Size { get; set; }

    public List<HandmadeColorDto> Colors { get; set; } = new();
    public string? OtherColor { get; set; }

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Center { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<HandmadeImageDto> Images { get; set; } = new();
    public HandmadeVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Handmade;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
