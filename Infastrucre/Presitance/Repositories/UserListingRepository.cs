using Microsoft.EntityFrameworkCore;
using Persistence.Listings;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Repositories;

public class UserListingRepository : IUserListingRepository
{
    private readonly IReadOnlyList<IUserListingSource> _sources;

    public UserListingRepository(IEnumerable<IUserListingSource> sources)
    {
        _sources = sources.ToList();
        SupportedModules = _sources.Select(source => source.Type).ToHashSet();
    }

    public IReadOnlySet<ListingModuleType> SupportedModules { get; }

    public async Task<(IReadOnlyList<UserListingRow> Items, int TotalCount)> GetPagedAsync(
        string userId,
        UserListingFilterParams filter,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(userId, filter.Type, filter.Status, utcNow);

        if (query is null)
            return (Array.Empty<UserListingRow>(), 0);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(row => row.CreatedAt)
            .ThenByDescending(row => row.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<UserListingStatusCount>> GetStatusCountsAsync(
        string userId,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(userId, type: null, status: null, utcNow);

        if (query is null)
            return Array.Empty<UserListingStatusCount>();

        var rows = await query
            .GroupBy(row => new { row.Type, row.Status })
            .Select(group => new
            {
                group.Key.Type,
                group.Key.Status,
                Count = group.Count()
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new UserListingStatusCount
            {
                Type = row.Type,
                Status = row.Status,
                Count = row.Count
            })
            .ToList();
    }

    public async Task<UserListingRow?> GetByKeyAsync(
        ListingModuleType type, Guid listingId, DateTime utcNow,
        CancellationToken cancellationToken = default,
        bool includeUnmoderated = false)
    {
        var source = _sources.FirstOrDefault(s => s.Type == type);

        if (source is null)
            return null;

        return await source.Query(ownerId: null, utcNow, includeUnmoderated)
            .FirstOrDefaultAsync(row => row.Id == listingId, cancellationToken);
    }

    public async Task<IReadOnlyList<UserListingRow>> GetByKeysAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        if (keys.Count == 0)
            return Array.Empty<UserListingRow>();

        var byModule = keys
            .GroupBy(key => key.Type)
            .ToDictionary(group => group.Key, group => group.Select(k => k.ListingId).Distinct().ToList());

        IQueryable<UserListingRow>? combined = null;

        foreach (var source in _sources)
        {
            if (!byModule.TryGetValue(source.Type, out var ids))
                continue;

            var query = source.Query(ownerId: null, utcNow)
                .Where(row => ids.Contains(row.Id));

            combined = combined is null ? query : combined.Concat(query);
        }

        if (combined is null)
            return Array.Empty<UserListingRow>();

        return await combined.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<(ListingModuleType Type, Guid ListingId)>> GetOwnedKeysAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        string ownerId,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        if (keys.Count == 0 || string.IsNullOrEmpty(ownerId))
            return Array.Empty<(ListingModuleType, Guid)>();

        var byModule = keys
            .GroupBy(key => key.Type)
            .ToDictionary(group => group.Key, group => group.Select(k => k.ListingId).Distinct().ToList());

        IQueryable<ListingKeyRow>? combined = null;

        foreach (var source in _sources)
        {
            if (!byModule.TryGetValue(source.Type, out var ids))
                continue;

            var query = source.Query(ownerId, utcNow)
                .Where(row => ids.Contains(row.Id))
                .Select(row => new ListingKeyRow { Type = row.Type, Id = row.Id });

            combined = combined is null ? query : combined.Concat(query);
        }

        if (combined is null)
            return Array.Empty<(ListingModuleType, Guid)>();

        var rows = await combined.ToListAsync(cancellationToken);

        return rows.Select(row => (row.Type, row.Id)).ToList();
    }

    public async Task<(IReadOnlyList<UserListingRow> Items, int TotalCount)> GetSimilarAsync(
        UserListingRow source,
        int pageIndex,
        int pageSize,
        DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        var category = source.CategoryId is { } categoryId
            ? (CategoryType)categoryId
            : ListingModuleCatalog.CategoryOf(source.Type);

        var siblingModules = ListingModuleCatalog.IsIsolated(source.Type)
            ? _sources.Where(s => s.Type == source.Type).ToList()
            : _sources
                .Where(s => s.Type == source.Type ||
                    (!ListingModuleCatalog.IsIsolated(s.Type) &&
                     ListingModuleCatalog.CategoryOf(s.Type) == category))
                .ToList();

        if (siblingModules.Count == 0)
            return (Array.Empty<UserListingRow>(), 0);

        IQueryable<UserListingRow>? combined = null;

        foreach (var sibling in siblingModules)
        {
            var query = sibling.Query(ownerId: null, utcNow)
                .Where(row => row.Status == ListingStatus.Active);

            if (sibling.Type == source.Type)
                query = query.Where(row => row.Id != source.Id);

            combined = combined is null ? query : combined.Concat(query);
        }

        if (combined is null)
            return (Array.Empty<UserListingRow>(), 0);

        var totalCount = await combined.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<UserListingRow>(), 0);

        var sameSubCategory = source.SubCategoryId;
        var sameModule = source.Type;
        var price = source.Price;

        var ordered = combined
            .OrderBy(row =>
                sameSubCategory != null
                    ? (row.SubCategoryId == sameSubCategory ? 0 : 1)
                    : (row.Type == sameModule ? 0 : 1))
            .ThenBy(row =>
                price == null || row.Price == null
                    ? 0m
                    : (row.Price > price ? row.Price - price : price - row.Price))
            .ThenByDescending(row => row.CreatedAt)
            .ThenByDescending(row => row.Id);

        var items = await ordered
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<(ListingModuleType Type, Guid ListingId)>> GetOwnedKeysAsync(
        string userId, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(userId, type: null, status: null, utcNow);

        if (query is null)
            return Array.Empty<(ListingModuleType, Guid)>();

        var rows = await query
            .Select(row => new { row.Type, row.Id })
            .ToListAsync(cancellationToken);

        return rows.Select(row => (row.Type, row.Id)).ToList();
    }

    private IQueryable<UserListingRow>? BuildQuery(
        string? ownerId, ListingModuleType? type, ListingStatus? status, DateTime utcNow)
    {
        IQueryable<UserListingRow>? combined = null;

        foreach (var source in _sources)
        {
            if (type is { } requested && source.Type != requested)
                continue;

            var query = source.Query(ownerId, utcNow);

            if (status is { } requestedStatus)
                query = query.Where(row => row.Status == requestedStatus);

            combined = combined is null ? query : combined.Concat(query);
        }

        return combined;
    }
}
