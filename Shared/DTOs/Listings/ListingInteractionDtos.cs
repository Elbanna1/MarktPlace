using System.Text.Json.Serialization;
using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Listings;

public class ListingKeyParams
{
    public ListingModuleType Type { get; set; } = ListingModuleType.Advertisement;
}

public class ListingCountersDto
{
    public int Views { get; set; }

    public int FavoriteCount { get; set; }

    public bool IsFavorite { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public int? MyRating { get; set; }
}

public class ListingViewResultDto : IListingViews
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TotalViews { get; set; }

    [JsonIgnore]
    public ListingModuleType ViewsListingType { get; set; }

    [JsonIgnore]
    public Guid ViewsListingId { get; set; }

    int? IListingViews.Views
    {
        get => TotalViews;
        set => TotalViews = value;
    }

    public DateTime? LastViewedAt { get; set; }

    public bool Counted { get; set; }
}

public class ListingFavoriteResultDto
{
    public bool IsFavorite { get; set; }

    public int FavoriteCount { get; set; }
}

public class ListingCardDto : IListingViews
{
    public Guid Id { get; set; }

    public ListingModuleType Type { get; set; }

    public string TypeName { get; set; } = default!;

    public string TypeNameAr { get; set; } = default!;

    public string Route { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string? MainImageUrl { get; set; }

    public decimal? Price { get; set; }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;

    public int SubCategoryId { get; set; }
    public string SubCategoryName { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public int FavoriteCount { get; set; }

    public bool IsFavorite { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? LastViewedAt { get; set; }

    ListingModuleType IListingViews.ViewsListingType => Type;

    Guid IListingViews.ViewsListingId => Id;
}

public class ListingCardFilterParams : PaginationParams
{
    public ListingModuleType? Type { get; set; }
}

public class SimilarListingFilterParams : PaginationParams
{
    public ListingModuleType Type { get; set; } = ListingModuleType.Advertisement;
}

public class ListingHistoryClearedDto
{
    public int RemovedCount { get; set; }
}

public class ListingActionsDto : IListingViews
{
    public Guid Id { get; set; }

    public ListingModuleType Type { get; set; }

    public string TypeName { get; set; } = default!;

    public string TypeNameAr { get; set; } = default!;

    public string Route { get; set; } = default!;

    public string Title { get; set; } = default!;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = default!;

    public bool IsOwner { get; set; }

    public ListingStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Views { get; set; }

    public int FavoriteCount { get; set; }

    public bool IsFavorite { get; set; }

    public bool HasReported { get; set; }

    public bool CanEdit { get; set; }

    public bool CanDelete { get; set; }

    public bool CanFavorite { get; set; }

    public bool CanReport { get; set; }

    public bool CanRepublish { get; set; }

    public bool CanShare { get; set; } = true;

    public string? DeepLink { get; set; }

    ListingModuleType IListingViews.ViewsListingType => Type;

    Guid IListingViews.ViewsListingId => Id;
}

public class CreateListingReportRequest
{
    public ListingModuleType Type { get; set; } = ListingModuleType.Advertisement;

    public ListingReportReason Reason { get; set; }

    public string? Details { get; set; }
}

public class ListingReportDto
{
    public Guid Id { get; set; }

    public ListingModuleType ListingType { get; set; }
    public string ListingTypeName { get; set; } = default!;
    public Guid ListingId { get; set; }

    public string? ListingTitle { get; set; }

    public ListingReportReason Reason { get; set; }

    public string ReasonName { get; set; } = default!;

    public string? Details { get; set; }

    public ListingReportStatus Status { get; set; }

    public string StatusName { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReporterUserId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReporterName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ListingOwnerId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? ReviewedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminNote { get; set; }
}

public class ListingReportFilterParams : PaginationParams
{
    public ListingReportStatus? Status { get; set; }

    public ListingReportReason? Reason { get; set; }

    public ListingModuleType? Type { get; set; }
}

public class UpdateListingReportRequest
{
    public ListingReportStatus Status { get; set; }

    public string? AdminNote { get; set; }
}

public class ListingReportOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public class ListingReportMetadataDto
{
    public IReadOnlyList<ListingReportOptionDto> Reasons { get; set; } = Array.Empty<ListingReportOptionDto>();

    public IReadOnlyList<ListingReportOptionDto> Statuses { get; set; } = Array.Empty<ListingReportOptionDto>();
}

public class ListingModuleDto
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string NameAr { get; set; } = default!;

    public string Route { get; set; } = default!;

    public int CategoryId { get; set; }

    public int SubCategoryId { get; set; }

    public bool SupportsRepublish { get; set; }
}

public class ListingInteractionMetadataDto
{
    public IReadOnlyList<ListingModuleDto> Modules { get; set; } = Array.Empty<ListingModuleDto>();

    public IReadOnlyList<ListingReportOptionDto> ReportReasons { get; set; } = Array.Empty<ListingReportOptionDto>();

    public IReadOnlyList<ListingReportOptionDto> ReportStatuses { get; set; } = Array.Empty<ListingReportOptionDto>();

    public int ViewDedupeWindowMinutes { get; set; }

    public int RecentlyViewedRetentionCount { get; set; }
}
