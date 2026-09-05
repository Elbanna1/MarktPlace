using Shared.Constants;
using Shared.Enums;

namespace Shared.DTOs.Listings;

public class UserListingDto
{
    public Guid Id { get; set; }

    public string Type { get; set; } = default!;

    public ListingModuleType TypeId { get; set; }

    public string Route { get; set; } = default!;

    public string Title { get; set; } = default!;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = default!;

    public string? MainImageUrl { get; set; }

    public decimal? Price { get; set; }

    public string Status { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? ExpireAt { get; set; }

    public int? RemainingDays { get; set; }

    public bool IsExpired { get; set; }

    public bool CanRepublish { get; set; }

    public bool CanUpdate { get; set; } = true;

    public bool CanDelete { get; set; } = true;

    public int Views { get; set; }

    public int FavoriteCount { get; set; }

    public decimal? AverageRating { get; set; }

    public int RatingsCount { get; set; }

    public bool IsFavorite { get; set; }

    public ExpiredSinceDto? ExpiredSince { get; set; }

    public ListingModerationDto? Moderation { get; set; }
}

public class ListingModerationDto
{
    public string Status { get; set; } = default!;

    public string StatusName { get; set; } = default!;

    public ListingRejectionReason? Reason { get; set; }

    public string? ReasonName { get; set; }

    public string? Notes { get; set; }

    public DateTime? DecidedAt { get; set; }

    public static ListingModerationDto From(UserListingRow row) => new()
    {
        Status = row.ModerationStatus.ToString(),
        StatusName = ModerationCatalog.NameOf(row.ModerationStatus),
        Reason = row.RejectionReason,
        ReasonName = row.RejectionReason is { } reason ? ModerationCatalog.NameOf(reason) : null,
        Notes = row.ModerationNotes,
        DecidedAt = row.ModeratedAt
    };

    public static ListingModerationDto? ForOwner(UserListingRow row) =>
        row.ModerationStatus == ModerationStatus.Approved ? null : From(row);
}

public class ExpiredSinceDto
{
    public int Days { get; set; }

    public DateTime ExpiredAt { get; set; }

    public string Text { get; set; } = default!;
}
