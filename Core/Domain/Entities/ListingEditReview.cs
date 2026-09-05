using Shared.Enums;

namespace Domain.Entities;

public static class ListingEditReview
{
    public static readonly IReadOnlySet<string> SystemOwnedProperties = new HashSet<string>(StringComparer.Ordinal)
    {
        "Id", "UserId", "OwnerId",

        "ModerationStatus", "ModeratedAt", "ModeratedBy", "RejectionReason", "ModerationNotes",

        "PublishedAt", "ExpireAt", "ExpiredAt", "FirstPublishedAt", "RepublishCount", "Status",

        "CreatedAt", "UpdatedAt", "IsDeleted", "DeletedAt",

        "Views", "ViewCount", "LikesCount", "CommentsCount",

        "IsFeatured", "IsPremium", "IsUrgent"
    };

    public static bool RequiresReviewAfterEdit(ModerationStatus status) =>
        status is ModerationStatus.Approved or ModerationStatus.Rejected;

    public static bool SendBackToReview(IModeratedListing listing)
    {
        if (!RequiresReviewAfterEdit(listing.ModerationStatus))
            return false;

        listing.ModerationStatus = ModerationStatus.Pending;

        listing.ModeratedAt = null;
        listing.ModeratedBy = null;
        listing.RejectionReason = null;
        listing.ModerationNotes = null;

        return true;
    }
}
