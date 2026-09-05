using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Common;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Shared.DTOs.Charity;

public class CharityImageDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = default!;
    public bool IsPrimary { get; set; }

    public int SortOrder { get; set; }
}

public abstract class CreateCharityRequestBase
{
    public string Phone { get; set; } = default!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public bool IsResponsibilityAccepted { get; set; }

    public List<IFormFile> Images { get; set; } = new();
}

public interface ICharityGalleryEdit
{
    List<Guid> RemoveImageIds { get; set; }
}

public abstract class CharityDetailsDtoBase : IListingStats
{
    public Guid Id { get; set; }

    public string OwnerId { get; set; } = default!;

    public string OwnerName { get; set; } = default!;

    public string Phone { get; set; } = default!;

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public bool HasLocation { get; set; }

    public bool IsResponsibilityAccepted { get; set; }
    public DateTime? ResponsibilityAcceptedAt { get; set; }

    public List<CharityImageDto> Images { get; set; } = new();

    public string? PrimaryImageUrl { get; set; }

    public ListingModuleType ListingType { get; set; }

    public string ListingTypeName { get; set; } = default!;

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; } = default!;

    public bool IsUrgent { get; set; }

    public string ShareUrl { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public DateTime? ApprovedAt
    {
        get => _approvedAt;

        set => _approvedAt = value is { } moment
            ? DateTime.SpecifyKind(moment, DateTimeKind.Utc)
            : null;
    }

    private DateTime? _approvedAt;

    public int ViewCount { get; set; }

    [JsonIgnore]
    public abstract ListingModuleType StatsListingType { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }
}

public abstract class CharityFilterParamsBase : PaginationParams
{
    public string? Search { get; set; }

    public bool? HasLocation { get; set; }

    public CharitySortBy SortBy { get; set; } = CharitySortBy.Newest;
}
