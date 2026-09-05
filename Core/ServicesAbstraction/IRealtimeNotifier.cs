using Shared.DTOs.Notifications;

namespace ServicesAbstraction;

public interface IRealtimeNotifier
{
    Task NotifyAsync(string userId, NotificationDto notification, CancellationToken cancellationToken = default);

    Task NotifyUnreadCountAsync(string userId, int unreadCount, CancellationToken cancellationToken = default);
}
