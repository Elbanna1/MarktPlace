using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Enums;

namespace Services.Listings;

public class ListingEditReviewNotifier : IListingEditReviewNotifier
{
    private readonly IListingEditReviewQueue _queue;
    private readonly INotificationService _notifications;
    private readonly IAdminAlertService _alerts;
    private readonly ILogger<ListingEditReviewNotifier> _logger;

    public ListingEditReviewNotifier(
        IListingEditReviewQueue queue,
        INotificationService notifications,
        IAdminAlertService alerts,
        ILogger<ListingEditReviewNotifier> logger)
    {
        _queue = queue;
        _notifications = notifications;
        _alerts = alerts;
        _logger = logger;
    }

    public async Task AnnounceAsync(CancellationToken cancellationToken = default)
    {
        var entries = _queue.Drain();

        if (entries.Count == 0)
            return;

        foreach (var entry in entries)
        {
            try
            {
                await _notifications.NotifyAsync(
                    entry.OwnerUserId, entry.Type, NotificationAction.EditedPendingReview,
                    entry.Id, entry.Title);

                await _alerts.NotifyPendingListingAsync(
                    entry.Type, entry.Id, entry.Title, entry.ReturnedAtUtc, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,
                    "{Module} {ListingId} was returned to the review queue but could not be announced.",
                    entry.Type, entry.Id);
            }
        }
    }
}
