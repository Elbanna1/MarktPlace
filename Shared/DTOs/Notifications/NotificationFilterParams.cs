using Shared.DTOs.Common;
using Shared.Enums;

namespace Shared.DTOs.Notifications;

public class NotificationFilterParams : PaginationParams
{
    public bool UnreadOnly { get; set; }

    public string? Search { get; set; }

    public NotificationType? Type { get; set; }

    public string? ReferenceType { get; set; }
}
