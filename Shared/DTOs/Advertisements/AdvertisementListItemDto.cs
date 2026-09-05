using Shared.DTOs.Listings;
using Shared.Enums;
using System.Text.Json.Serialization;
namespace Shared.DTOs.Advertisements;

public class AdvertisementListItemDto : IListingViews
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public decimal? Price { get; set; }
    public bool Negotiable { get; set; }

    public string ListingType { get; set; } = default!;
    public string Status { get; set; } = default!;

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; } = default!;

    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int? ManufacturingYear { get; set; }

    public string? BusinessName { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public int FavoriteCount { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public int? RemainingDays { get; set; }

    public bool IsExpired { get; set; }

    public string? PrimaryImageUrl { get; set; }
    public List<AdvertisementImageDto> Images { get; set; } = new();
    public List<FeatureDto> Features { get; set; } = new();

    public OwnerDto Owner { get; set; } = default!;

    ListingModuleType IListingViews.ViewsListingType => ListingModuleType.Advertisement;

    Guid IListingViews.ViewsListingId => Id;
}
