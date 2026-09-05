using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MarkatPlace.RealTime;

[Authorize]
public class NotificationHub : Hub
{
    public const string ReceiveNotification = "ReceiveNotification";

    public const string UnreadCountChanged = "UnreadCountChanged";
}
