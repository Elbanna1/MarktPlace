using Shared.DTOs.Listings;
using Shared.Enums;

namespace ServicesAbstraction;

public interface IUserListingRepository
{
    IReadOnlySet<ListingModuleType> SupportedModules { get; }

    Task<(IReadOnlyList<UserListingRow> Items, int TotalCount)> GetPagedAsync(
        string userId,
        UserListingFilterParams filter,
        DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UserListingStatusCount>> GetStatusCountsAsync(
        string userId,
        DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task<UserListingRow?> GetByKeyAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow,
        CancellationToken cancellationToken = default,
        bool includeUnmoderated = false);

    Task<IReadOnlyList<UserListingRow>> GetByKeysAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<(ListingModuleType Type, Guid ListingId)>> GetOwnedKeysAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        string ownerId,
        DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<UserListingRow> Items, int TotalCount)> GetSimilarAsync(
        UserListingRow source,
        int pageIndex,
        int pageSize,
        DateTime utcNow,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<(ListingModuleType Type, Guid ListingId)>> GetOwnedKeysAsync(
        string userId, DateTime utcNow, CancellationToken cancellationToken = default);
}
