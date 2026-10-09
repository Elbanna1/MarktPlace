using Microsoft.AspNetCore.SignalR;
using ServicesAbstraction;
using Shared.DTOs.Realtime;

namespace MarkatPlace.RealTime;

public class SignalRListingEventPublisher : IListingEventPublisher
{
    private readonly IHubContext<NotificationHub> _hub;
    private readonly ILogger<SignalRListingEventPublisher> _logger;

    public SignalRListingEventPublisher(
        IHubContext<NotificationHub> hub, ILogger<SignalRListingEventPublisher> logger)
    {
        _hub = hub;
        _logger = logger;
    }

    public async Task PublishAsync(IReadOnlyList<ListingChange> changes, CancellationToken cancellationToken = default)
    {
        foreach (var (ownerUserId, change) in changes)
        {
            try
            {
                if (!string.IsNullOrEmpty(ownerUserId))
                {
                    await _hub.Clients.User(ownerUserId)
                        .SendAsync(NotificationHub.ListingChanged, change, cancellationToken);
                }

                await _hub.Clients.Group(NotificationHub.AdminsGroup)
                    .SendAsync(NotificationHub.ListingChanged, change, cancellationToken);

                await _hub.Clients.Group(NotificationHub.AdminsGroup)
                    .SendAsync(NotificationHub.ModerationQueueChanged, new ModerationQueueChangedEventDto
                    {
                        Id = change.Id,
                        Type = change.Type,
                        Change = change.Change,
                        ModerationStatus = change.ModerationStatus,
                        OccurredAt = change.OccurredAt
                    }, cancellationToken);

                if (change.IsPublic || change.WasPublic)
                {
                    await _hub.Clients.All.SendAsync(
                        NotificationHub.PublicListingChanged,
                        PublicListingChangedEventDto.From(change),
                        cancellationToken);
                }
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                _logger.LogWarning(exception,
                    "Real-time delivery of the {Change} event for {Module} {ListingId} failed; clients will catch up on their next fetch.",
                    change.Change, change.Type, change.Id);
            }
        }
    }
}
