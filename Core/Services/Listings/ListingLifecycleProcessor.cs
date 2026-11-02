using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Realtime;
using Shared.Enums;

namespace Services.Listings;

public sealed class ListingLifecycleProcessor : IListingLifecycleProcessor
{
    private static readonly TimeSpan ExpiredNoticeLookback = TimeSpan.FromHours(48);

    private readonly IListingLifecycleRepository _lifecycle;
    private readonly IAdvertisementRepository _advertisements;
    private readonly INotificationService _notifications;
    private readonly IAdminAlertService _alerts;
    private readonly IReadOnlyList<IListingEventPublisher> _publishers;
    private readonly ILogger<ListingLifecycleProcessor> _logger;

    public ListingLifecycleProcessor(
        IListingLifecycleRepository lifecycle,
        IAdvertisementRepository advertisements,
        INotificationService notifications,
        IAdminAlertService alerts,
        IEnumerable<IListingEventPublisher> publishers,
        ILogger<ListingLifecycleProcessor> logger)
    {
        _lifecycle = lifecycle;
        _advertisements = advertisements;
        _notifications = notifications;
        _alerts = alerts;
        _publishers = publishers.ToList();
        _logger = logger;
    }

    public async Task ProcessAsync(DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var generic = (IReadOnlyList<ExpiringListingRow>)Array.Empty<ExpiringListingRow>();
        var advertisements = (IReadOnlyList<ExpiringListingRow>)Array.Empty<ExpiringListingRow>();

        var lastDay = Math.Max(
            AdminRepublishPolicy.WarningDays,
            AdvertisementConstants.ExpiryWarningDays.Max() + 1);

        var horizon = utcNow.AddDays(lastDay);

        await RunStepAsync("scanning the generic modules", async () =>
        {
            generic = await _lifecycle.GetWindowsClosingBetweenAsync(
                utcNow - ExpiredNoticeLookback, horizon.AddTicks(1), cancellationToken);
        }, cancellationToken);

        await RunStepAsync("scanning the advertisements", async () =>
        {
            var rows = await _advertisements.GetAdvertisementsExpiringBetweenAsync(utcNow, horizon);

            advertisements = rows
                .Select(ad => new ExpiringListingRow(
                    ListingModuleType.Advertisement, ad.Id, ad.OwnerId, ad.Title, ad.ExpireAt!.Value,
                    ad.PublishedAt, ad.CategoryId, ad.SubCategoryId))
                .ToList();
        }, cancellationToken);

        await RunStepAsync("announcing expired listings", async () =>
        {
            var expired = generic.Where(row => row.ExpireAt <= utcNow).ToList();
            var notified = await NotifyOwnersExpiredAsync(expired, cancellationToken);

            await PublishExpiredAsync(notified, utcNow, cancellationToken);
        }, cancellationToken);

        var upcoming = generic.Where(row => row.ExpireAt > utcNow).Concat(advertisements).ToList();

        await RunStepAsync("warning the owners", () =>
            WarnOwnersAsync(upcoming, utcNow, cancellationToken), cancellationToken);

        await RunStepAsync("alerting the administrators", async () =>
        {
            var until = utcNow.AddDays(AdminRepublishPolicy.WarningDays);
            var expiring = upcoming.Where(row => row.ExpireAt <= until).ToList();
            var sent = await _alerts.NotifyListingsExpiringAsync(expiring, utcNow, cancellationToken);

            if (sent > 0)
                _logger.LogInformation("Sent {Count} administrator expiry alert(s).", sent);
        }, cancellationToken);

        await RunStepAsync("expiring the advertisements", async () =>
        {
            var expired = await _advertisements.ExpireDueAdvertisementsAsync(utcNow, cancellationToken);

            await NotifyOwnersExpiredAsync(expired, cancellationToken);
            await PublishExpiredAsync(expired, utcNow, cancellationToken);

            if (expired.Count > 0)
                _logger.LogInformation("Expired {Count} advertisement(s).", expired.Count);
        }, cancellationToken);
    }

    private async Task RunStepAsync(
        string step, Func<Task> action, CancellationToken cancellationToken)
    {
        try
        {
            await action();
        }
        catch (Exception exception) when (exception is not OperationCanceledException ||
                                          !cancellationToken.IsCancellationRequested)
        {
            _logger.LogError(exception, "Listing lifecycle step failed while {Step}.", step);
        }
    }

    private async Task WarnOwnersAsync(
        IReadOnlyList<ExpiringListingRow> upcoming, DateTime utcNow, CancellationToken cancellationToken)
    {
        foreach (var days in AdvertisementConstants.ExpiryWarningDays)
        {
            var from = utcNow.AddDays(days);
            var to = from.AddDays(1);

            var rows = upcoming.Where(row => row.ExpireAt >= from && row.ExpireAt < to).ToList();

            var (type, message) = days == 1
                ? (NotificationType.AdvertisementExpiringTomorrow, "⏰ ينتهي إعلانك غدًا.")
                : (NotificationType.AdvertisementExpiringSoon, $"⏰ سينتهي إعلانك خلال {days} أيام.");

            await NotifyOwnersAsync(
                rows, type, "إعلانك على وشك الانتهاء", message, "⏰", cancellationToken);
        }
    }

    private Task<IReadOnlyList<ExpiringListingRow>> NotifyOwnersExpiredAsync(
        IReadOnlyList<ExpiringListingRow> rows, CancellationToken cancellationToken) =>
        NotifyOwnersAsync(
            rows,
            NotificationType.AdvertisementExpired,
            "انتهاء مدة الإعلان",
            "⌛ انتهت مدة إعلانك. أعد نشره ليظهر مرة أخرى.",
            "⌛",
            cancellationToken);

    private async Task<IReadOnlyList<ExpiringListingRow>> NotifyOwnersAsync(
        IReadOnlyList<ExpiringListingRow> rows,
        NotificationType type,
        string title,
        string message,
        string icon,
        CancellationToken cancellationToken)
    {
        var notified = new List<ExpiringListingRow>();

        foreach (var row in rows)
        {
            try
            {
                var subject = NotificationCatalog.For(row.Type);

                var notice = new NotificationContent(
                        title,
                        $"{message} ({row.Title})",
                        icon,
                        type,
                        subject.ReferenceType,
                        subject.EntityName,
                        $"{subject.Route}/{row.Id}")
                    .WithListing(
                        listingType: row.Type,
                        categoryId: CategoryOf(row),
                        subCategoryId: SubCategoryOf(row));

                var created = await _notifications.CreateIfNotExistsAsync(
                    row.OwnerId,
                    notice,
                    row.Id,
                    createdAfterUtc: row.PublishedAt,
                    action: NotificationAction.Expired,
                    cancellationToken: cancellationToken);

                if (created)
                    notified.Add(row);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                _logger.LogError(exception,
                    "Failed to send a lifecycle notice for {Module} {ListingId}.", row.Type, row.Id);
            }
        }

        if (notified.Count > 0)
            _logger.LogInformation("Sent {Count} lifecycle notice(s) of type {Type}.", notified.Count, type);

        return notified;
    }

    private async Task PublishExpiredAsync(
        IReadOnlyList<ExpiringListingRow> rows, DateTime utcNow, CancellationToken cancellationToken)
    {
        if (rows.Count == 0 || _publishers.Count == 0)
            return;

        var changes = rows
            .Select(row => new ListingChange(row.OwnerId, new ListingChangedEventDto
            {
                Id = row.Id,
                Type = row.Type,
                TypeName = row.Type.ToString(),
                Route = ListingModuleCatalog.RouteOf(row.Type),
                CategoryId = CategoryOf(row),
                SubCategoryId = SubCategoryOf(row),
                Change = ListingChangeKinds.Expired,
                Status = ListingStatus.Expired,
                ModerationStatus = ModerationStatus.Approved,
                IsPublic = false,
                WasPublic = true,
                PublishedAt = row.PublishedAt,
                ExpireAt = row.ExpireAt,
                RemainingDays = 0,
                OccurredAt = utcNow
            }))
            .ToList();

        foreach (var publisher in _publishers)
        {
            try
            {
                await publisher.PublishAsync(changes, cancellationToken);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                _logger.LogWarning(exception, "Publishing {Count} expiry event(s) failed.", changes.Count);
            }
        }
    }

    private static int? CategoryOf(ExpiringListingRow row) =>
        row.CategoryId ?? (int?)ListingModuleCatalog.CategoryOf(row.Type);

    private static int? SubCategoryOf(ExpiringListingRow row) =>
        row.SubCategoryId ?? (int?)ListingModuleCatalog.SubCategoryOf(row.Type);
}
