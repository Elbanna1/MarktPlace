using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Listings;

public class RateListingRequest
{
    public int? Rating { get; set; }
}

public class ListingRatingTargetDto
{
    public ListingModuleType Type { get; set; }

    public string TypeName { get; set; } = default!;

    public string TypeNameAr { get; set; } = default!;

    public Guid Id { get; set; }

    public string? Title { get; set; }

    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }

    public int? SubCategoryId { get; set; }
    public string? SubCategoryName { get; set; }

    public string? ImageUrl { get; set; }

    public string Route { get; set; } = default!;

    public string DetailsUrl { get; set; } = default!;

    public bool IsAvailable { get; set; }
}

public class ListingRatingReviewerDto
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;
}

public class ListingRatingDto
{
    public Guid Id { get; set; }

    public int Rating { get; set; }

    public ListingRatingReviewerDto Reviewer { get; set; } = default!;

    public ListingRatingTargetDto Target { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class ListingRatingResultDto
{
    public ListingRatingDto Rating { get; set; } = default!;

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }

    public bool Created { get; set; }
}

public class ListingRatingSummaryDto
{
    public ListingModuleType ListingType { get; set; }

    public Guid ListingId { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }

    public int? MyRating { get; set; }

    public IReadOnlyDictionary<int, int> Breakdown { get; set; } = new Dictionary<int, int>();
}

public class ListingRatingMetadataDto
{
    public int MinRating { get; set; }

    public int MaxRating { get; set; }

    public IReadOnlyList<ListingModuleDto> Modules { get; set; } = Array.Empty<ListingModuleDto>();
}

public class ListingRatingFilterParams : PaginationParams
{
    public ListingModuleType? ListingType { get; set; }

    public Guid? ListingId { get; set; }

    public string? ReviewerUserId { get; set; }

    public int? Rating { get; set; }

    public int? MinRating { get; set; }

    public int? MaxRating { get; set; }
}
