using Shared.Enums;

namespace Shared.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public NotificationType Type { get; set; }

    public NotificationAction Action { get; set; }

    public string? Icon { get; set; }

    public string? EntityName { get; set; }

    public string? DeepLink { get; set; }

    public Guid? ReferenceId { get; set; }

    public string? ReferenceType { get; set; }

    public ListingModuleType? ListingType { get; set; }

    public int? CategoryId { get; set; }

    public int? SubCategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public string? CategoryName { get; set; }

    public string? SubCategoryName { get; set; }

    public string? ListingTitle { get; set; }

    public string? OwnerId { get; set; }

    public string? OwnerName { get; set; }

    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
}
