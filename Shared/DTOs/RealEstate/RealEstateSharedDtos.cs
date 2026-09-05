using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.Enums;
using Shared.DTOs.Listings;

namespace Shared.DTOs.RealEstate;

public class RealEstateImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }
}

public abstract class CreateRealEstateRequestBase
{
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string AdvertiserName { get; set; } = default!;

    public List<IFormFile> Images { get; set; } = new();

    public IFormFile? Video { get; set; }

    public RealEstateListingType ListingType { get; set; }

    public string Center { get; set; } = default!;

    public RealEstateProject? Project { get; set; }

    public string? OtherProject { get; set; }

    public string? District { get; set; }

    public string Address { get; set; } = default!;

    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;

    public string? WhatsApp { get; set; }

    public string? Email { get; set; }

    public bool? Negotiable { get; set; }

    public string? Notes { get; set; }
}

public interface IRealEstateGalleryEdit
{
    List<Guid> RemoveImageIds { get; set; }

    bool RemoveVideo { get; set; }
}

public interface IRealEstateNamedPayload
{
    string ListingTypeName { get; set; }

    string? ProjectName { get; set; }
}

public abstract class RealEstateDetailsDtoBase : IRealEstateNamedPayload, IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string AdvertiserName { get; set; } = default!;

    public List<RealEstateImageDto> Images { get; set; } = new();

    public string? VideoUrl { get; set; }

    public RealEstateListingType ListingType { get; set; }

    public string ListingTypeName { get; set; } = default!;

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;

    public RealEstateProject? Project { get; set; }

    public string? ProjectName { get; set; }

    public string? OtherProject { get; set; }
    public string? District { get; set; }
    public string Address { get; set; } = default!;
    public string? GoogleMaps { get; set; }

    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }
    public string? Email { get; set; }

    public bool Negotiable { get; set; }
    public string? Notes { get; set; }

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

public abstract class RealEstateListItemDtoBase : IRealEstateNamedPayload, IListingStats
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public string AdvertiserName { get; set; } = default!;

    public RealEstateListingType ListingType { get; set; }
    public string ListingTypeName { get; set; } = default!;

    public string Governorate { get; set; } = default!;
    public string Center { get; set; } = default!;
    public RealEstateProject? Project { get; set; }
    public string? ProjectName { get; set; }
    public string? District { get; set; }

    public string Phone { get; set; } = default!;
    public string? WhatsApp { get; set; }

    public bool Negotiable { get; set; }

    public string? PrimaryImageUrl { get; set; }

    public List<RealEstateImageDto> Images { get; set; } = new();

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

public abstract class RealEstateFilterParamsBase : PaginationParams
{
    public string? Search { get; set; }

    public RealEstateListingType? ListingType { get; set; }

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public string? Governorate { get; set; }

    public string? Center { get; set; }

    public RealEstateProject? Project { get; set; }

    public bool? Negotiable { get; set; }

    public bool? IsFeatured { get; set; }

    public bool? IsPremium { get; set; }

    public bool? IsUrgent { get; set; }

    public RealEstateSortBy SortBy { get; set; } = RealEstateSortBy.Newest;
}

public class RealEstatePriceStatisticsDto
{
    public int Count { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public decimal? AveragePrice { get; set; }
}

public class RealEstateSuggestionDto
{
    public string Term { get; set; } = default!;

    public int Count { get; set; }
}

public class ReorderRealEstateImagesRequest
{
    public List<Guid> ImageIds { get; set; } = new();
}

public class PromoteRealEstateRequest
{
    public bool IsFeatured { get; set; }

    public bool IsPremium { get; set; }

    public bool IsUrgent { get; set; }
}
