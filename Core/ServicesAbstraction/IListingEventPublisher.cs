using Shared.DTOs.Realtime;

namespace ServicesAbstraction;

public interface IListingEventPublisher
{
    Task PublishAsync(IReadOnlyList<ListingChange> changes, CancellationToken cancellationToken = default);
}
