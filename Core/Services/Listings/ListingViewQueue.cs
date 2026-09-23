using System.Collections.Concurrent;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Constants;
using Shared.Settings;

namespace Services.Listings;

public sealed class ListingViewQueue : IListingViewRecorder
{
    public const int Capacity = 20_000;

    private const int AnonymousKeyCapacity = 200_000;

    private readonly Channel<ListingViewEvent> _channel =
        Channel.CreateBounded<ListingViewEvent>(new BoundedChannelOptions(Capacity)
        {
            // Wait, not DropWrite: TryWrite still returns immediately without blocking the
            // request, but it reports the refusal instead of silently swallowing the event, which
            // is what makes the loss visible in the log rather than invisible in the counts.
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        });

    private readonly ConcurrentDictionary<string, DateTime> _anonymousSeen = new(StringComparer.Ordinal);

    private readonly ListingInteractionSettings _settings;
    private readonly ILogger<ListingViewQueue> _logger;

    private long _dropped;
    private DateTime _lastPrunedUtc = DateTime.UtcNow;

    public ListingViewQueue(
        IOptions<ListingInteractionSettings> settings, ILogger<ListingViewQueue> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public ChannelReader<ListingViewEvent> Reader => _channel.Reader;

    public long Dropped => Interlocked.Read(ref _dropped);

    public bool Record(ListingViewEvent view)
    {
        // An anonymous viewer has no ListingViewers row to update, so a view that would not be
        // counted anyway carries no information. Dropping it here keeps the queue — and the table —
        // from growing with repeat traffic from the same address.
        if (view.ViewerUserId is null && !ShouldCountAnonymous(view))
            return false;

        if (_channel.Writer.TryWrite(view))
            return true;

        var dropped = Interlocked.Increment(ref _dropped);

        if (dropped == 1 || dropped % 1000 == 0)
        {
            _logger.LogWarning(
                "The listing-view queue is full; {Dropped} view events have been dropped. Views are " +
                "recorded on a best-effort basis and the queue holds {Capacity} events.",
                dropped, Capacity);
        }

        return false;
    }

    public void Complete() => _channel.Writer.TryComplete();

    private bool ShouldCountAnonymous(ListingViewEvent view)
    {
        if (string.IsNullOrWhiteSpace(view.IpAddress))
            return false;

        Prune(view.OccurredAtUtc);

        var key = AnonymousKey(view);

        var counted = true;

        _anonymousSeen.AddOrUpdate(
            key,
            _ => view.OccurredAtUtc,
            (_, seenAt) =>
            {
                if (seenAt + _settings.ViewDedupeWindow > view.OccurredAtUtc)
                {
                    counted = false;
                    return seenAt;
                }

                return view.OccurredAtUtc;
            });

        return counted;
    }

    private static string AnonymousKey(ListingViewEvent view)
    {
        var key = $"{(int)view.Type}:{view.ListingId:N}:" +
                  $"{ListingInteractionCatalog.AnonymousViewerKeyPrefix}{view.IpAddress}";

        return key.Length <= ListingInteractionCatalog.ViewerKeyMaxLength + 48
            ? key
            : key[..(ListingInteractionCatalog.ViewerKeyMaxLength + 48)];
    }

    private void Prune(DateTime utcNow)
    {
        if (_anonymousSeen.Count < AnonymousKeyCapacity && _lastPrunedUtc + _settings.ViewDedupeWindow > utcNow)
            return;

        lock (_anonymousSeen)
        {
            if (_lastPrunedUtc + TimeSpan.FromMinutes(1) > utcNow && _anonymousSeen.Count < AnonymousKeyCapacity)
                return;

            _lastPrunedUtc = utcNow;

            var cutoff = utcNow - _settings.ViewDedupeWindow;

            foreach (var entry in _anonymousSeen)
            {
                if (entry.Value <= cutoff)
                    _anonymousSeen.TryRemove(entry.Key, out _);
            }

            // A flood of distinct addresses must never be allowed to grow without bound; when
            // pruning by age is not enough, the whole window is forfeited instead.
            if (_anonymousSeen.Count >= AnonymousKeyCapacity)
                _anonymousSeen.Clear();
        }
    }
}
