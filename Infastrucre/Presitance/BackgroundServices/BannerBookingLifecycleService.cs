using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Shared.Settings;

namespace Persistence.BackgroundServices;

public class BannerBookingLifecycleService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    private static readonly TimeSpan StartupDelay = TimeSpan.FromSeconds(45);

    private static readonly TimeSpan PreviewRetention = TimeSpan.FromHours(24);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BannerBookingLifecycleService> _logger;
    private readonly FileStorageSettings _storage;

    public BannerBookingLifecycleService(
        IServiceScopeFactory scopeFactory,
        ILogger<BannerBookingLifecycleService> logger,
        IOptions<FileStorageSettings> storage)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _storage = storage.Value;
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
                _logger.LogError(ex, "Banner booking lifecycle processing failed.");
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
        var repository = scope.ServiceProvider.GetRequiredService<IBannerBookingRepository>();
        var notifications = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var now = DateTime.UtcNow;

        await PublishDueBookingsAsync(repository, notifications, now, cancellationToken);
        await ExpireFinishedBookingsAsync(repository, notifications, now, cancellationToken);

        PurgeStalePreviews(now);
    }

    private async Task PublishDueBookingsAsync(
        IBannerBookingRepository repository,
        INotificationService notifications,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var due = await repository.GetBookingsToPublishAsync(now, cancellationToken);
        if (due.Count == 0)
            return;

        foreach (var booking in due)
        {
            booking.Status = BannerBookingStatus.Published;
            booking.PublishedAt = now;
            booking.UpdatedAt = now;
        }

        await repository.SaveChangesAsync(cancellationToken);

        foreach (var booking in due)
        {
            var content = NotificationCatalog.BannerBookings.Published(
                booking.Id,
                booking.Title,
                BannerBookingCatalog.GetLocationName(booking.Location),
                booking.EndDate);

            await notifications.CreateIfNotExistsAsync(
                booking.UserId,
                content.Title,
                content.Message,
                content.Type,
                booking.Id,
                content.ReferenceType,

                createdAfterUtc: booking.ApprovedAt,
                action: NotificationAction.Approved,
                icon: content.Icon,
                entityName: content.EntityName,
                deepLink: content.DeepLink);
        }

        _logger.LogInformation("Published {Count} banner booking(s).", due.Count);
    }

    private async Task ExpireFinishedBookingsAsync(
        IBannerBookingRepository repository,
        INotificationService notifications,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var finished = await repository.GetBookingsToExpireAsync(now, cancellationToken);
        if (finished.Count == 0)
            return;

        foreach (var booking in finished)
        {
            booking.Status = BannerBookingStatus.Expired;
            booking.ExpiredAt = now;
            booking.UpdatedAt = now;
        }

        await repository.SaveChangesAsync(cancellationToken);

        foreach (var booking in finished)
        {
            var content = NotificationCatalog.BannerBookings.Expired(booking.Id, booking.Title, booking.DurationDays);

            await notifications.CreateIfNotExistsAsync(
                booking.UserId,
                content.Title,
                content.Message,
                content.Type,
                booking.Id,
                content.ReferenceType,
                createdAfterUtc: booking.PublishedAt,
                action: NotificationAction.Expired,
                icon: content.Icon,
                entityName: content.EntityName,
                deepLink: content.DeepLink);
        }

        _logger.LogInformation("Expired {Count} banner booking(s).", finished.Count);
    }

    private void PurgeStalePreviews(DateTime now)
    {
        try
        {
            var previewFolder = Path.Combine(ResolveUploadsRoot(), ImageConstants.BannerBookingPreviewFolder);

            if (!Directory.Exists(previewFolder))
                return;

            var cutoff = now - PreviewRetention;
            var removed = 0;

            foreach (var file in Directory.EnumerateFiles(previewFolder))
            {
                if (File.GetLastWriteTimeUtc(file) > cutoff)
                    continue;

                try
                {
                    File.Delete(file);
                    removed++;
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
            }

            if (removed > 0)
                _logger.LogInformation("Removed {Count} stale banner preview file(s).", removed);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Sweeping stale banner preview files failed.");
        }
    }

    private string ResolveUploadsRoot()
    {
        var root = _storage.UploadsRootPath;

        if (!string.IsNullOrWhiteSpace(root))
            return Path.GetFullPath(root);

        return Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "wwwroot", ImageConstants.UploadsRootFolder));
    }
}
