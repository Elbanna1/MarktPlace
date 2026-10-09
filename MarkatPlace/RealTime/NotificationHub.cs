using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Constants;

namespace MarkatPlace.RealTime;

[AllowAnonymous]
public class NotificationHub : Hub
{
    public const string ReceiveNotification = "ReceiveNotification";

    public const string UnreadCountChanged = "UnreadCountChanged";

    public const string ListingChanged = "ListingChanged";

    public const string PublicListingChanged = "PublicListingChanged";

    public const string ModerationQueueChanged = "ModerationQueueChanged";

    public const string AdminsGroup = "admins";

    public override async Task OnConnectedAsync()
    {
        if (Context.User?.IsInRole(AppRoles.Admin) == true)
            await Groups.AddToGroupAsync(Context.ConnectionId, AdminsGroup);

        await base.OnConnectedAsync();
    }
}
