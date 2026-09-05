using System.Text.Json.Serialization;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.Antiques;

public class PaintingImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }
}

public class PaintingVideoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
}

public class PaintingDetailsDto : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string PaintingName { get; set; } = default!;

    public PaintingType PaintingType { get; set; }

    public string PaintingTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public string ArtistName { get; set; } = default!;
    public int? ExecutionYear { get; set; }

    public decimal? Width { get; set; }
    public decimal? Height { get; set; }

    public PaintingMaterial Material { get; set; }

    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public bool Framed { get; set; }

    public PaintingOriginality Originality { get; set; }

    public string OriginalityName { get; set; } = default!;

    public bool SignedByArtist { get; set; }

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<PaintingImageDto> Images { get; set; } = new();
    public PaintingVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Painting;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public class PaintingListItemDto : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string PaintingName { get; set; } = default!;

    public PaintingType PaintingType { get; set; }
    public string PaintingTypeName { get; set; } = default!;
    public string? OtherType { get; set; }

    public string ArtistName { get; set; } = default!;
    public int? ExecutionYear { get; set; }

    public decimal? Width { get; set; }
    public decimal? Height { get; set; }

    public PaintingMaterial Material { get; set; }
    public string MaterialName { get; set; } = default!;
    public string? OtherMaterial { get; set; }

    public bool Framed { get; set; }

    public PaintingOriginality Originality { get; set; }
    public string OriginalityName { get; set; } = default!;

    public bool SignedByArtist { get; set; }

    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Center { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }
    public List<PaintingImageDto> Images { get; set; } = new();
    public PaintingVideoDto? Video { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ListingModuleType StatsListingType => ListingModuleType.Painting;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}
