using Domain.Entities;
using Shared.Enums;

namespace ServicesAbstraction;

public record ExpiringListingRow(
    ListingModuleType Type,
    Guid Id,
    string OwnerId,
    string Title,
    DateTime ExpireAt,
    DateTime? PublishedAt,
    int? CategoryId = null,
    int? SubCategoryId = null);

public interface IListingLifecycleRepository
{
    Task<IExpiringListing?> FindAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default);

    Task SaveAsync(ListingModuleType type, CancellationToken cancellationToken = default);

    Task<T> RunExclusiveAsync<T>(
        ListingModuleType type, Guid id, Func<Task<T>> operation,
        CancellationToken cancellationToken = default);

    void MarkAsNew(object listing, DateTime utcNow);

    Task<IReadOnlyList<ExpiringListingRow>> GetWindowsClosingBetweenAsync(
        DateTime fromUtc, DateTime toUtc, CancellationToken cancellationToken = default);
}
