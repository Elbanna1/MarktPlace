using Domain.Entities.Listings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Enums;
using Shared.Settings;

namespace Services.Listings;

public sealed class ListingViewFlushService : BackgroundService
{
    private static readonly TimeSpan FlushInterval = TimeSpan.FromSeconds(5);

    private static readonly TimeSpan ShutdownDrainBudget = TimeSpan.FromSeconds(20);

    private const int MaxEventsPerFlush = 2_000;

    private readonly ListingViewQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ListingInteractionSettings _settings;
    private readonly ILogger<ListingViewFlushService> _logger;

    public ListingViewFlushService(
        ListingViewQueue queue,
        IServiceScopeFactory scopeFactory,
        IOptions<ListingInteractionSettings> settings,
        ILogger<ListingViewFlushService> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(FlushInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            await FlushSafelyAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // Views queued since the last tick would otherwise be lost on every deployment. The queue
        // is closed to new writers first so the drain is guaranteed to terminate.
        _queue.Complete();

        using var budget = new CancellationTokenSource(ShutdownDrainBudget);

        while (_queue.Reader.TryPeek(out _) && !budget.IsCancellationRequested)
        {
            if (!await FlushSafelyAsync(budget.Token))
                break;
        }

        if (_queue.Reader.TryPeek(out _))
        {
            _logger.LogWarning(
                "Shutting down with listing-view events still queued; they were not written. " +
                "The drain budget is {Seconds}s.", ShutdownDrainBudget.TotalSeconds);
        }

        await base.StopAsync(cancellationToken);
    }

    private async Task<bool> FlushSafelyAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await FlushAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Flushing queued listing views failed; the batch was dropped.");
            return false;
        }
    }

    private async Task<bool> FlushAsync(CancellationToken cancellationToken)
    {
        var batch = new List<ListingViewEvent>();

        while (batch.Count < MaxEventsPerFlush && _queue.Reader.TryRead(out var view))
            batch.Add(view);

        if (batch.Count == 0)
            return false;

        using var scope = _scopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IListingInteractionRepository>();
        var listings = scope.ServiceProvider.GetRequiredService<IUserListingRepository>();

        var now = DateTime.UtcNow;

        foreach (var group in batch.GroupBy(view => (view.Type, view.ListingId)))
        {
            var (type, listingId) = group.Key;

            var listing = await listings.GetByKeyAsync(type, listingId, now, cancellationToken);

            if (listing is null)
                continue;

            var counted = 0;

            foreach (var view in group.Where(view => view.ViewerUserId is not null))
            {
                if (await ApplyViewerAsync(repository, listing.OwnerId, view, now, cancellationToken))
                    counted++;
            }

            // Anonymous events only reach the queue when they already passed its dedupe window.
            counted += group.Count(view => view.ViewerUserId is null);

            if (counted == 0)
                continue;

            await UpsertCounterAsync(repository, type, listingId, counted, now, cancellationToken);

            if (group.Any(view => view.IncrementModuleCounter))
                await repository.IncrementModuleViewCountAsync(type, listingId, counted, cancellationToken);
        }

        await repository.SaveChangesAsync(cancellationToken);

        foreach (var userId in batch
                     .Where(view => view.ViewerUserId is not null)
                     .Select(view => view.ViewerUserId!)
                     .Distinct(StringComparer.Ordinal))
        {
            await repository.TrimRecentlyViewedAsync(
                userId, _settings.RecentlyViewedRetentionCount, cancellationToken);
        }

        return true;
    }

    private async Task<bool> ApplyViewerAsync(
        IListingInteractionRepository repository,
        string ownerId,
        ListingViewEvent view,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var viewerKey = Truncate(view.ViewerUserId!);

        var viewer = await repository.GetViewerAsync(view.Type, view.ListingId, viewerKey, cancellationToken);

        var isOwnerViewing = string.Equals(view.ViewerUserId, ownerId, StringComparison.Ordinal);

        var counts = !isOwnerViewing &&
                     (viewer is null || viewer.LastViewedAt + _settings.ViewDedupeWindow <= now);

        if (viewer is null)
        {
            repository.AddViewer(new ListingViewer
            {
                ListingType = view.Type,
                ListingId = view.ListingId,
                ViewerKey = viewerKey,
                UserId = view.ViewerUserId,
                FirstViewedAt = now,
                LastViewedAt = now,
                ViewCount = counts ? 1 : 0
            });
        }
        else
        {
            viewer.LastViewedAt = now;

            if (counts)
                viewer.ViewCount++;
        }

        return counts;
    }

    private static async Task UpsertCounterAsync(
        IListingInteractionRepository repository,
        ListingModuleType type,
        Guid listingId,
        int delta,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var counter = await repository.GetCounterAsync(type, listingId, cancellationToken);

        if (counter is null)
        {
            repository.AddCounter(new ListingViewCounter
            {
                ListingType = type,
                ListingId = listingId,
                TotalViews = delta,
                LastViewedAt = now
            });

            return;
        }

        await repository.IncrementViewsAsync(type, listingId, delta, now, cancellationToken);
    }

    private static string Truncate(string value) =>
        value.Length <= ListingInteractionCatalog.ViewerKeyMaxLength
            ? value
            : value[..ListingInteractionCatalog.ViewerKeyMaxLength];
}
