using Shared.Enums;

namespace Domain.Entities;

public class UserNotificationInterest
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public int CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public bool IsEnabled { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}

public class UserNotificationPreference
{
    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public bool NewListingsEnabled { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}

public class ListingNotificationDispatch
{
    public ListingModuleType ListingType { get; set; }

    public Guid ListingId { get; set; }

    public DateTime DispatchedAt { get; set; } = DateTime.UtcNow;

    public int RecipientCount { get; set; }
}
