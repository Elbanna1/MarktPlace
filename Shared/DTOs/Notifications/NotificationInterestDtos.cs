namespace Shared.DTOs.Notifications;

public class NotificationInterestDto
{
    public Guid Id { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public int? SubCategoryId { get; set; }

    public string? SubCategoryName { get; set; }

    public bool IsWholeCategory => SubCategoryId is null;

    public bool IsEnabled { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class NotificationInterestsDto
{
    public bool NewListingsEnabled { get; set; }

    public List<NotificationInterestDto> Interests { get; set; } = new();
}

public class NotificationInterestOptionsDto
{
    public bool NewListingsEnabled { get; set; }

    public List<NotificationInterestCategoryOptionDto> Categories { get; set; } = new();
}

public class NotificationInterestCategoryOptionDto
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public string NameAr { get; set; } = default!;
    public string? Icon { get; set; }

    public bool IsSelected { get; set; }

    public Guid? InterestId { get; set; }

    public bool IsEnabled { get; set; } = true;

    public List<NotificationInterestSubCategoryOptionDto> SubCategories { get; set; } = new();
}

public class NotificationInterestSubCategoryOptionDto
{
    public int SubCategoryId { get; set; }
    public string Name { get; set; } = default!;
    public string NameAr { get; set; } = default!;
    public string? Icon { get; set; }

    public bool IsSelected { get; set; }

    public Guid? InterestId { get; set; }

    public bool IsEnabled { get; set; } = true;

    public bool CoveredByCategory { get; set; }
}

public class AddNotificationInterestRequest
{
    public int CategoryId { get; set; }

    public int? SubCategoryId { get; set; }
}

public class ReplaceNotificationInterestsRequest
{
    public List<AddNotificationInterestRequest> Interests { get; set; } = new();

    public bool? NewListingsEnabled { get; set; }
}

public class UpdateNotificationInterestRequest
{
    public bool IsEnabled { get; set; }
}

public class UpdateNotificationPreferencesRequest
{
    public bool NewListingsEnabled { get; set; }
}

public class NotificationPreferencesDto
{
    public bool NewListingsEnabled { get; set; }
}
