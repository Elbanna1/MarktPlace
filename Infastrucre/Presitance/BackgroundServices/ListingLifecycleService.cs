using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Realtime;
using Shared.Enums;

namespace Persistence.BackgroundServices;

public class ListingLifecycleService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);
    private static readonly TimeSpan StartupDelay = TimeSpan.FromSeconds(15);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ListingLifecycleService> _logger;

    public ListingLifecycleService(
        IServiceScopeFactory scopeFactory,
        ILogger<ListingLifecycleService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(StartupDelay, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        using var timer = new PeriodicTimer(Interval);

        do
        {
            try
            {
                await ProcessLifecycleAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Advertisement lifecycle processing failed.");
            }
        }
        while (await SafeWaitForNextTickAsync(timer, stoppingToken));
    }

    private static async Task<bool> SafeWaitForNextTickAsync(PeriodicTimer timer, CancellationToken token)
    {
        try
        {
            return await timer.WaitForNextTickAsync(token);
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    private async Task ProcessLifecycleAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IAdvertisementRepository>();
        var notifications = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var fileService = scope.ServiceProvider.GetRequiredService<IFileService>();

        var lifecycle = scope.ServiceProvider.GetRequiredService<IListingLifecycleRepository>();

        var now = DateTime.UtcNow;

        var publishers = scope.ServiceProvider.GetServices<IListingEventPublisher>().ToList();

        await ProcessGenericModulesAsync(lifecycle, notifications, publishers, now, cancellationToken);

        await SendExpiryWarningsAsync(repository, notifications, now);

        var toExpire = await repository.GetAdvertisementsToExpireAsync(now);
        foreach (var ad in toExpire)
        {
            ad.Status = AdvertisementStatus.Expired;
            ad.ExpiredAt = now;
        }

        if (toExpire.Count > 0)
        {
            await repository.SaveChangesAsync();

            foreach (var ad in toExpire)
            {
                await notifications.CreateIfNotExistsAsync(
                    ad.OwnerId,
                    "انتهاء مدة الإعلان",
                    "⌛ انتهت مدة إعلانك. أعد نشره ليظهر مرة أخرى.",
                    NotificationType.AdvertisementExpired,
                    ad.Id,
                    NotificationReferenceTypes.Advertisement,

                    createdAfterUtc: ad.PublishedAt,
                    action: NotificationAction.Expired,
                    icon: "⌛",
                    entityName: AdvertisementSubject.EntityName,
                    deepLink: $"{AdvertisementSubject.Route}/{ad.Id}");
            }

            _logger.LogInformation("Expired {Count} advertisement(s).", toExpire.Count);
        }

        var toDelete = await repository.GetAdvertisementsToDeleteAsync(now);
        foreach (var ad in toDelete)
        {
            foreach (var image in ad.Images)
                fileService.Delete(image.ImagePath);

            repository.Remove(ad);
        }

        if (toDelete.Count > 0)
        {
            var removed = toDelete.Select(ad => (ad.OwnerId, ad.Id, ad.Title)).ToList();

            await repository.SaveChangesAsync();

            foreach (var (ownerId, adId, adTitle) in removed)
            {
                await notifications.CreateAsync(
                    ownerId,
                    "حذف الإعلان",
                    $"🗑️ تم حذف إعلانك نهائيًا بعد انتهاء مهلة إعادة النشر. ({adTitle})",
                    NotificationType.AdvertisementDeleted,
                    adId,
                    NotificationReferenceTypes.Advertisement,
                    action: NotificationAction.Deleted,
                    icon: "🗑️",
                    entityName: AdvertisementSubject.EntityName);
            }

            _logger.LogInformation("Deleted {Count} expired advertisement(s) past the grace period.", toDelete.Count);
        }
    }

    private async Task ProcessGenericModulesAsync(
        IListingLifecycleRepository lifecycle,
        INotificationService notifications,
        IReadOnlyList<IListingEventPublisher> publishers,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var expired = await NotifyGenericAsync(
            lifecycle, notifications, now.AddHours(-2), now,
            (subject, _) => (NotificationType.AdvertisementExpired,
                             "انتهاء مدة الإعلان",
                             "⌛ انتهت مدة إعلانك. أعد نشره ليظهر مرة أخرى.",
                             "⌛"),
            cancellationToken);

        await PublishExpiredAsync(publishers, expired, now, cancellationToken);

        foreach (var days in AdvertisementConstants.ExpiryWarningDays)
        {
            var windowStart = now.AddDays(days);

            await NotifyGenericAsync(
                lifecycle, notifications, windowStart, windowStart.AddDays(1),
                (_, offset) => offset == 1
                    ? (NotificationType.AdvertisementExpiringTomorrow,
                       "إعلانك على وشك الانتهاء", "⏰ ينتهي إعلانك غدًا.", "⏰")
                    : (NotificationType.AdvertisementExpiringSoon,
                       "إعلانك على وشك الانتهاء", $"⏰ سينتهي إعلانك خلال {offset} أيام.", "⏰"),
                cancellationToken,
                offsetDays: days);
        }
    }

    private async Task PublishExpiredAsync(
        IReadOnlyList<IListingEventPublisher> publishers,
        IReadOnlyList<ExpiringListingRow> rows,
        DateTime now,
        CancellationToken cancellationToken)
    {
        if (rows.Count == 0 || publishers.Count == 0)
            return;

        var changes = rows
            .Select(row => new ListingChange(row.OwnerId, new ListingChangedEventDto
            {
                Id = row.Id,
                Type = row.Type,
                TypeName = row.Type.ToString(),
                Route = ListingModuleCatalog.RouteOf(row.Type),
                CategoryId = (int?)ListingModuleCatalog.CategoryOf(row.Type),
                SubCategoryId = (int?)ListingModuleCatalog.SubCategoryOf(row.Type),
                Change = ListingChangeKinds.Expired,
                Status = ListingStatus.Expired,
                ModerationStatus = ModerationStatus.Approved,
                IsPublic = false,
                WasPublic = true,
                PublishedAt = row.PublishedAt,
                ExpireAt = row.ExpireAt,
                RemainingDays = 0,
                OccurredAt = now
            }))
            .ToList();

        foreach (var publisher in publishers)
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

    private async Task<IReadOnlyList<ExpiringListingRow>> NotifyGenericAsync(
        IListingLifecycleRepository lifecycle,
        INotificationService notifications,
        DateTime from,
        DateTime to,
        Func<NotificationSubject, int, (NotificationType Type, string Title, string Message, string Icon)> content,
        CancellationToken cancellationToken,
        int offsetDays = 0)
    {
        var rows = await lifecycle.GetWindowsClosingBetweenAsync(from, to, cancellationToken);

        if (rows.Count == 0)
            return Array.Empty<ExpiringListingRow>();

        var notified = new List<ExpiringListingRow>();

        foreach (var row in rows)
        {
            var subject = NotificationCatalog.For(row.Type);
            var (type, title, message, icon) = content(subject, offsetDays);

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
                    categoryId: (int?)ListingModuleCatalog.CategoryOf(row.Type),
                    subCategoryId: (int?)ListingModuleCatalog.SubCategoryOf(row.Type));

            var created = await notifications.CreateIfNotExistsAsync(
                row.OwnerId,
                notice,
                row.Id,
                createdAfterUtc: row.PublishedAt,
                action: NotificationAction.Expired,
                cancellationToken: cancellationToken);

            if (created)
                notified.Add(row);
        }

        if (notified.Count > 0)
            _logger.LogInformation("Sent {Count} lifecycle notice(s) across the generic modules.", notified.Count);

        return notified;
    }

    private async Task SendExpiryWarningsAsync(
        IAdvertisementRepository repository, INotificationService notifications, DateTime now)
    {
        foreach (var days in AdvertisementConstants.ExpiryWarningDays)
        {
            var windowStart = now.AddDays(days);
            var windowEnd = windowStart.AddDays(1);

            var expiring = await repository.GetAdvertisementsExpiringBetweenAsync(windowStart, windowEnd);
            if (expiring.Count == 0)
                continue;

            var (type, title, message) = WarningContent(days);
            var sent = 0;

            foreach (var ad in expiring)
            {
                var created = await notifications.CreateIfNotExistsAsync(
                    ad.OwnerId,
                    title,
                    message,
                    type,
                    ad.Id,
                    NotificationReferenceTypes.Advertisement,
                    createdAfterUtc: ad.PublishedAt,
                    action: NotificationAction.Expired,
                    icon: "⏰",
                    entityName: AdvertisementSubject.EntityName,
                    deepLink: $"{AdvertisementSubject.Route}/{ad.Id}");

                if (created)
                    sent++;
            }

            if (sent > 0)
                _logger.LogInformation("Sent {Count} '{Days}-day' expiry warning(s).", sent, days);
        }
    }

    private static readonly NotificationSubject AdvertisementSubject =
        NotificationCatalog.For(ListingModuleType.Advertisement);

    private static (NotificationType Type, string Title, string Message) WarningContent(int days) => days switch
    {
        1 => (NotificationType.AdvertisementExpiringTomorrow,
              "إعلانك على وشك الانتهاء",
              "ينتهي إعلانك غدًا."),
        _ => (NotificationType.AdvertisementExpiringSoon,
              "إعلانك على وشك الانتهاء",
              $"سينتهي إعلانك خلال {days} أيام.")
    };
}
