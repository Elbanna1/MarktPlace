using Shared.Enums;

namespace Shared.DTOs.Listings;

public class UserListingRow
{
    public Guid Id { get; set; }

    public ListingModuleType Type { get; set; }

    public string OwnerId { get; set; } = default!;

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string Title { get; set; } = default!;

    public string? MainImageUrl { get; set; }

    public decimal? Price { get; set; }

    public ListingStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public bool SupportsRepublish { get; set; }

    public ModerationStatus ModerationStatus { get; set; }

    public ListingRejectionReason? RejectionReason { get; set; }

    public string? ModerationNotes { get; set; }

    public DateTime? ModeratedAt { get; set; }
}

public class UserListingStatusCount
{
    public ListingModuleType Type { get; set; }

    public ListingStatus Status { get; set; }

    public int Count { get; set; }
}
