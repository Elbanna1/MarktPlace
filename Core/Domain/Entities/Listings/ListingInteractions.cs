using Shared.Enums;

namespace Domain.Entities.Listings;

public class ListingViewCounter
{
    public ListingModuleType ListingType { get; set; }

    public Guid ListingId { get; set; }

    public int TotalViews { get; set; }

    public DateTime? LastViewedAt { get; set; }
}

public class ListingViewer
{
    public long Id { get; set; }

    public ListingModuleType ListingType { get; set; }

    public Guid ListingId { get; set; }

    public string ViewerKey { get; set; } = default!;

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public DateTime FirstViewedAt { get; set; }

    public DateTime LastViewedAt { get; set; }

    public int ViewCount { get; set; }
}

public class ListingFavorite
{
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public ListingModuleType ListingType { get; set; }

    public Guid ListingId { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class ListingRating
{
    public Guid Id { get; set; }

    public ListingModuleType ListingType { get; set; }

    public Guid ListingId { get; set; }

    public string ReviewerUserId { get; set; } = default!;
    public ApplicationUser Reviewer { get; set; } = default!;

    public string? ListingOwnerId { get; set; }

    public string? ListingTitle { get; set; }

    public int Rating { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class ListingReport
{
    public Guid Id { get; set; }

    public ListingModuleType ListingType { get; set; }

    public Guid ListingId { get; set; }

    public string ReporterUserId { get; set; } = default!;
    public ApplicationUser Reporter { get; set; } = default!;

    public string? ListingOwnerId { get; set; }

    public string? ListingTitle { get; set; }

    public ListingReportReason Reason { get; set; }

    public string? Details { get; set; }

    public ListingReportStatus Status { get; set; } = ListingReportStatus.Pending;

    public DateTime CreatedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewedByUserId { get; set; }

    public string? AdminNote { get; set; }
}
