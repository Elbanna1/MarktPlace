using Domain.Entities;
using Shared.Enums;

namespace ServicesAbstraction;

public record ExpiringListingRow(
    ListingModuleType Type,
    Guid Id,
    string OwnerId,
    string Title,
    DateTime ExpireAt,
    DateTime? PublishedAt);

public interface IListingLifecycleRepository
{
    Task<IExpiringListing?> FindAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default);

    Task SaveAsync(ListingModuleType type, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExpiringListingRow>> GetWindowsClosingBetweenAsync(
        DateTime fromUtc, DateTime toUtc, CancellationToken cancellationToken = default);
}
