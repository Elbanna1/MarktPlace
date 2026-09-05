using ServicesAbstraction;
using Shared.Enums;

namespace Services.Listings;

public sealed class ListingEditReviewQueue : IListingEditReviewQueue
{
    private readonly Dictionary<(ListingModuleType Type, Guid Id), ListingEditReviewEntry> _entries = new();

    public void Enqueue(ListingEditReviewEntry entry)
    {
        _entries[(entry.Type, entry.Id)] = entry;
    }

    public IReadOnlyList<ListingEditReviewEntry> Drain()
    {
        if (_entries.Count == 0)
            return Array.Empty<ListingEditReviewEntry>();

        var drained = _entries.Values.ToList();
        _entries.Clear();

        return drained;
    }
}
