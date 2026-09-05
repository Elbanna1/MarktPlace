using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Antiques;

public class CoinStampImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class CoinStampVideoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
}

public class CoinStampDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string ItemName { get; set; } = default!;

    public CoinStampItemType ItemType { get; set; }

    public string ItemTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public string? Country { get; set; }
    public int? IssueYear { get; set; }
    public string? Denomination { get; set; }

    public CoinStampMetal Metal { get; set; }

    public string MetalName { get; set; } = default!;
    public string? OtherMetal { get; set; }

    public CoinStampCondition Condition { get; set; }

    public string ConditionName { get; set; } = default!;

    public bool IsOriginal { get; set; }
    public bool IsRare { get; set; }
    public bool HasCertificate { get; set; }

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<CoinStampImageDto> Images { get; set; } = new();
    public CoinStampVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.CoinStamp;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class CoinStampListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string ItemName { get; set; } = default!;

    public CoinStampItemType ItemType { get; set; }
    public string ItemTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public string? Country { get; set; }
    public int? IssueYear { get; set; }
    public string? Denomination { get; set; }

    public CoinStampMetal Metal { get; set; }
    public string MetalName { get; set; } = default!;
    public string? OtherMetal { get; set; }

    public CoinStampCondition Condition { get; set; }
    public string ConditionName { get; set; } = default!;

    public bool IsOriginal { get; set; }
    public bool IsRare { get; set; }
    public bool HasCertificate { get; set; }

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Center { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<CoinStampImageDto> Images { get; set; } = new();
    public CoinStampVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.CoinStamp;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
