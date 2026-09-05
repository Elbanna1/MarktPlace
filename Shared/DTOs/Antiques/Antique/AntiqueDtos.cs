using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Antiques;

public class AntiqueImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class AntiqueVideoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
}

public class AntiqueDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string AntiqueName { get; set; } = default!;

    public AntiqueType AntiqueType { get; set; }

    public string AntiqueTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public int? ManufactureYear { get; set; }
    public string? CountryOfOrigin { get; set; }
    public string? Manufacturer { get; set; }

    public AntiqueMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public AntiqueCondition Condition { get; set; }

    public string ConditionName { get; set; } = default!;

    public AntiqueWorkingStatus WorkingStatus { get; set; }

    public string WorkingStatusName { get; set; } = default!;

    public AntiqueOriginality Originality { get; set; }

    public string OriginalityName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<AntiqueImageDto> Images { get; set; } = new();
    public AntiqueVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Antique;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class AntiqueListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string AntiqueName { get; set; } = default!;

    public AntiqueType AntiqueType { get; set; }
    public string AntiqueTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public int? ManufactureYear { get; set; }
    public string? CountryOfOrigin { get; set; }
    public string? Manufacturer { get; set; }

    public AntiqueMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public AntiqueCondition Condition { get; set; }
    public string ConditionName { get; set; } = default!;

    public AntiqueWorkingStatus WorkingStatus { get; set; }
    public string WorkingStatusName { get; set; } = default!;

    public AntiqueOriginality Originality { get; set; }
    public string OriginalityName { get; set; } = default!;

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Center { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<AntiqueImageDto> Images { get; set; } = new();
    public AntiqueVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Antique;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
