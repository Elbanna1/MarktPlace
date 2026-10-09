using Shared.Enums;

namespace Shared.DTOs.Realtime;

public static class ListingChangeKinds
{
    public const string Created = "created";

    public const string Updated = "updated";

    public const string Approved = "approved";

    public const string Rejected = "rejected";

    public const string Suspended = "suspended";

    public const string ReturnedToReview = "returnedToReview";

    public const string Republished = "republished";

    public const string Expired = "expired";

    public const string Deleted = "deleted";
}

public sealed class ListingChangedEventDto
{
    public Guid Id { get; set; }

    public ListingModuleType Type { get; set; }

    public string TypeName { get; set; } = default!;

    public string Route { get; set; } = default!;

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string Change { get; set; } = default!;

    public ListingStatus Status { get; set; }

    public ModerationStatus ModerationStatus { get; set; }

    public bool IsPublic { get; set; }

    public bool WasPublic { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpireAt { get; set; }

    public int? RemainingDays { get; set; }

    public int RepublishCount { get; set; }

    public DateTime OccurredAt { get; set; }
}

public sealed class PublicListingChangedEventDto
{
    public Guid Id { get; set; }

    public ListingModuleType Type { get; set; }

    public string Route { get; set; } = default!;

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string Change { get; set; } = default!;

    public bool IsPublic { get; set; }

    public DateTime OccurredAt { get; set; }

    public static PublicListingChangedEventDto From(ListingChangedEventDto change) =>
        new()
        {
            Id = change.Id,
            Type = change.Type,
            Route = change.Route,
            CategoryId = change.CategoryId,
            SubCategoryId = change.SubCategoryId,
            Change = change.Change,
            IsPublic = change.IsPublic,
            OccurredAt = change.OccurredAt
        };
}

public sealed class ModerationQueueChangedEventDto
{
    public Guid Id { get; set; }

    public ListingModuleType Type { get; set; }

    public string Change { get; set; } = default!;

    public ModerationStatus ModerationStatus { get; set; }

    public DateTime OccurredAt { get; set; }
}

public sealed record ListingChange(string OwnerUserId, ListingChangedEventDto Event);
