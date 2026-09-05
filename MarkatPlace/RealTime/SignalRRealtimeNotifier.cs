using Microsoft.AspNetCore.SignalR;
using ServicesAbstraction;
using Shared.DTOs.Notifications;

namespace MarkatPlace.RealTime;

public class SignalRRealtimeNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRRealtimeNotifier(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyAsync(string userId, NotificationDto notification, CancellationToken cancellationToken = default) =>
        _hubContext.Clients.User(userId).SendAsync(NotificationHub.ReceiveNotification, notification, cancellationToken);

    public Task NotifyUnreadCountAsync(string userId, int unreadCount, CancellationToken cancellationToken = default) =>
        _hubContext.Clients.User(userId).SendAsync(NotificationHub.UnreadCountChanged, unreadCount, cancellationToken);
}
