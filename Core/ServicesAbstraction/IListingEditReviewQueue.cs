using Shared.Enums;

namespace ServicesAbstraction;

public record ListingEditReviewEntry(
    ListingModuleType Type, Guid Id, string OwnerUserId, string? Title, DateTime ReturnedAtUtc);

public interface IListingEditReviewQueue
{
    void Enqueue(ListingEditReviewEntry entry);

    IReadOnlyList<ListingEditReviewEntry> Drain();
}

public interface IListingEditReviewNotifier
{
    Task AnnounceAsync(CancellationToken cancellationToken = default);
}
