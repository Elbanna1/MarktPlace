using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Antiques;

public class DecorAntiqueImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class DecorAntiqueVideoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
}

public class DecorAntiqueDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string ItemName { get; set; } = default!;

    public DecorAntiqueItemType ItemType { get; set; }

    public string ItemTypeName { get; set; } = default!;
    public string? OtherItemType { get; set; }

    public DecorAntiqueMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public DecorAntiqueCondition Condition { get; set; }

    public string ConditionName { get; set; } = default!;

    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }

    public DecorAntiqueOriginality Originality { get; set; }

    public string OriginalityName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<DecorAntiqueImageDto> Images { get; set; } = new();
    public DecorAntiqueVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.DecorAntique;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class DecorAntiqueListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string ItemName { get; set; } = default!;

    public DecorAntiqueItemType ItemType { get; set; }
    public string ItemTypeName { get; set; } = default!;
    public string? OtherItemType { get; set; }

    public DecorAntiqueMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public DecorAntiqueCondition Condition { get; set; }
    public string ConditionName { get; set; } = default!;

    public DecorAntiqueOriginality Originality { get; set; }
    public string OriginalityName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Center { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<DecorAntiqueImageDto> Images { get; set; } = new();
    public DecorAntiqueVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.DecorAntique;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
