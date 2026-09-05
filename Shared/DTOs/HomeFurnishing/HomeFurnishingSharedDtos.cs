using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.HomeFurnishing;

public class HomeFurnishingImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }
}

public abstract class CreateHomeFurnishingRequestBase
{
    public string SellerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public string ProductName { get; set; } = default!;

    public decimal Price { get; set; }

    public bool Negotiable { get; set; }

    public string Center { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }
}

public interface IHomeFurnishingGalleryEdit
{
    List<Guid> RemoveImageIds { get; set; }

    bool RemoveVideo { get; set; }
}

public abstract class HomeFurnishingDetailsDtoBase : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }
    public string? Email { get; set; }

    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public List<HomeFurnishingImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsPremium { get; set; }

    public bool IsUrgent { get; set; }

    public int ViewCount { get; set; }

    public string ShareUrl { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore]
    public abstract ListingModuleType StatsListingType { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public abstract class HomeFurnishingListItemDtoBase : IListingStats
{
    public Guid Id { get; set; }

    public string SellerName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public string ProductName { get; set; } = default!;
    public decimal Price { get; set; }
    public bool Negotiable { get; set; }

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string? PrimaryImageUrl { get; set; }

    public List<HomeFurnishingImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public bool IsFeatured { get; set; }
    public bool IsPremium { get; set; }
    public bool IsUrgent { get; set; }
    public int ViewCount { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public abstract ListingModuleType StatsListingType { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public abstract class HomeFurnishingFilterParamsBase : PaginationParams
{
    public string? Search { get; set; }

    public string? Center { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public bool? Negotiable { get; set; }

    public bool? IsFeatured { get; set; }

    public bool? IsPremium { get; set; }

    public bool? IsUrgent { get; set; }

    public HomeFurnishingSortBy SortBy { get; set; } = HomeFurnishingSortBy.Newest;
}

public class HomeFurnishingPriceStatisticsDto
{
    public int Count { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public decimal? AveragePrice { get; set; }
}

public class HomeFurnishingSuggestionDto
{
    public string Term { get; set; } = default!;

    public int Count { get; set; }
}

public class ReorderHomeFurnishingImagesRequest
{
    public List<Guid> ImageIds { get; set; } = new();
}

public class PromoteHomeFurnishingRequest
{
    public bool IsFeatured { get; set; }

    public bool IsPremium { get; set; }

    public bool IsUrgent { get; set; }
}
