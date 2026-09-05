using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Listings;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Admin;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Repositories;

public class AdminAdRepository : IAdminAdRepository
{
    private readonly AppDbContext _context;
    private readonly IReadOnlyList<IUserListingSource> _sources;
    private readonly IReadOnlyDictionary<ListingModuleType, IListingModerationSource> _moderators;

    public AdminAdRepository(
        AppDbContext context,
        IEnumerable<IUserListingSource> sources,
        IEnumerable<IListingModerationSource> moderators)
    {
        _context = context;
        _sources = sources.ToList();
        _moderators = moderators.ToDictionary(moderator => moderator.Type);
    }

    public async Task<(IReadOnlyList<UserListingRow> Items, int TotalCount)> GetPagedAsync(
        AdminAdFilterParams filter, DateTime utcNow, CancellationToken cancellationToken = default,
        bool includeTotal = true)
    {
        IReadOnlyList<string> matchedUserIds = Array.Empty<string>();
        var search = filter.Search?.Trim();

        if (!string.IsNullOrWhiteSpace(search))
            matchedUserIds = await FindUserIdsAsync(search, cancellationToken);

        var query = BuildQuery(filter, search, matchedUserIds, utcNow);

        if (query is null)
            return (Array.Empty<UserListingRow>(), 0);

        var totalCount = includeTotal ? await query.CountAsync(cancellationToken) : 0;

        if (includeTotal && totalCount == 0)
            return (Array.Empty<UserListingRow>(), 0);

        var items = await query
            .OrderByDescending(row => row.CreatedAt)
            .ThenByDescending(row => row.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<AdminAdStatusCount>> GetStatusBreakdownAsync(
        DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(
            new AdminAdFilterParams(), search: null, matchedUserIds: Array.Empty<string>(), utcNow);

        if (query is null)
            return Array.Empty<AdminAdStatusCount>();

        var rows = await query
            .GroupBy(row => new { row.Status, row.ModerationStatus })
            .Select(group => new
            {
                group.Key.Status,
                group.Key.ModerationStatus,
                Count = group.Count()
            })
            .ToListAsync(cancellationToken);

        return rows
            .Select(row => new AdminAdStatusCount(row.Status, row.ModerationStatus, row.Count))
            .ToList();
    }

    public async Task<int> CountByStatusAsync(
        ModerationStatus status, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var filter = new AdminAdFilterParams { ModerationStatus = status };

        var query = BuildQuery(filter, search: null, matchedUserIds: Array.Empty<string>(), utcNow);

        return query is null ? 0 : await query.CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        AdminAdFilterParams filter, DateTime utcNow, CancellationToken cancellationToken = default)
    {
        var query = BuildQuery(filter, search: null, matchedUserIds: Array.Empty<string>(), utcNow);

        return query is null ? 0 : await query.CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyDictionary<string, int>> CountByOwnersAsync(
        IReadOnlyCollection<string> ownerIds, DateTime utcNow,
        CancellationToken cancellationToken = default)
    {
        if (ownerIds.Count == 0)
            return new Dictionary<string, int>();

        IQueryable<UserListingRow>? combined = null;

        foreach (var source in _sources)
        {
            var query = source
                .Query(ownerId: null, utcNow, includeUnmoderated: true)
                .Where(row => ownerIds.Contains(row.OwnerId));

            combined = combined is null ? query : combined.Concat(query);
        }

        if (combined is null)
            return new Dictionary<string, int>();

        var counts = await combined
            .GroupBy(row => row.OwnerId)
            .Select(group => new { OwnerId = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return counts.ToDictionary(entry => entry.OwnerId, entry => entry.Count);
    }

    public async Task<IReadOnlyList<AdminModuleCount>> CountByModuleAsync(
        DateTime utcNow, CancellationToken cancellationToken = default)
    {
        IQueryable<UserListingRow>? combined = null;

        foreach (var source in _sources)
        {
            var query = source.Query(ownerId: null, utcNow, includeUnmoderated: true);

            combined = combined is null ? query : combined.Concat(query);
        }

        if (combined is null)
            return Array.Empty<AdminModuleCount>();

        var counts = await combined
            .GroupBy(row => new { row.Type, row.SubCategoryId })
            .Select(group => new
            {
                group.Key.Type,
                group.Key.SubCategoryId,
                Count = group.Count()
            })
            .ToListAsync(cancellationToken);

        return counts
            .Select(entry => new AdminModuleCount(entry.Type, entry.SubCategoryId, entry.Count))
            .ToList();
    }

    public async Task<IReadOnlyList<AdminTimeSeriesPointDto>> CountByDayAsync(
        DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        IQueryable<UserListingRow>? combined = null;

        foreach (var source in _sources)
        {
            var query = source
                .Query(ownerId: null, DateTime.UtcNow, includeUnmoderated: true)
                .Where(row => row.CreatedAt >= from && row.CreatedAt <= to);

            combined = combined is null ? query : combined.Concat(query);
        }

        if (combined is null)
            return Array.Empty<AdminTimeSeriesPointDto>();

        var rows = await combined
            .GroupBy(row => row.CreatedAt.Date)
            .Select(group => new AdminTimeSeriesPointDto { Date = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        return rows.OrderBy(point => point.Date).ToList();
    }

    public async Task<IReadOnlyDictionary<string, AdminAdOwner>> GetOwnersAsync(
        IReadOnlyCollection<string> userIds, CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
            return new Dictionary<string, AdminAdOwner>();

        var owners = await _context.Users
            .AsNoTracking()
            .Where(user => userIds.Contains(user.Id))
            .Select(user => new AdminAdOwner(
                user.Id,
                ((user.FirstName ?? string.Empty) + " " + (user.SecondName ?? string.Empty)).Trim(),
                user.PhoneNumber))
            .ToListAsync(cancellationToken);

        return owners.ToDictionary(owner => owner.UserId);
    }

    public async Task<IReadOnlyList<string>> FindUserIdsAsync(
        string search, CancellationToken cancellationToken = default)
    {
        var term = search.Trim();

        return await _context.Users
            .AsNoTracking()
            .Where(user =>
                EF.Functions.Like(user.FirstName, $"%{term}%") ||
                EF.Functions.Like(user.SecondName, $"%{term}%") ||
                (user.PhoneNumber != null && EF.Functions.Like(user.PhoneNumber, $"%{term}%")) ||
                (user.Email != null && EF.Functions.Like(user.Email, $"%{term}%")))
            .Select(user => user.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<IModeratedListing?> FindForModerationAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default) =>
        _moderators.TryGetValue(type, out var moderator)
            ? moderator.FindAsync(id, cancellationToken)
            : Task.FromResult<IModeratedListing?>(null);

    public Task SaveModerationAsync(
        ListingModuleType type, CancellationToken cancellationToken = default) =>
        _moderators.TryGetValue(type, out var moderator)
            ? moderator.SaveChangesAsync(cancellationToken)
            : Task.CompletedTask;

    public async Task<AdminListingSnapshot?> LoadListingAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await FindForModerationAsync(type, id, cancellationToken);

        if (listing is null)
            return null;

        var entry = _context.Entry(listing);

        var values = new Dictionary<string, object?>(StringComparer.Ordinal);

        foreach (var property in entry.Properties)
            values[property.Metadata.Name] = property.CurrentValue;

        var imageUrls = new List<string>();
        var videoUrls = new List<string>();

        foreach (var collection in entry.Collections)
        {
            var elementType = collection.Metadata.TargetEntityType.ClrType;

            var isImages = typeof(IListingImage).IsAssignableFrom(elementType);
            var isVideos = typeof(IListingVideo).IsAssignableFrom(elementType);

            if (!isImages && !isVideos)
                continue;

            var rows = LoadMedia(collection, elementType);

            if (isImages)
            {
                imageUrls.AddRange(rows
                    .Cast<IListingImage>()
                    .OrderByDescending(image => image.IsPrimary)
                    .ThenBy(image => image.CreatedAt)
                    .Select(image => image.ImageUrl));
            }
            else
            {
                videoUrls.AddRange(rows
                    .Cast<IListingVideo>()
                    .OrderBy(video => video.CreatedAt)
                    .Select(video => video.VideoUrl));
            }
        }

        return new AdminListingSnapshot(listing, values, imageUrls, videoUrls);
    }

    private static readonly System.Reflection.MethodInfo IgnoreNamedFiltersMethod =
        typeof(EntityFrameworkQueryableExtensions)
            .GetMethods()
            .Single(method =>
                method.Name == nameof(EntityFrameworkQueryableExtensions.IgnoreQueryFilters) &&
                method.GetParameters().Length == 2);

    private static List<object> LoadMedia(
        Microsoft.EntityFrameworkCore.ChangeTracking.CollectionEntry collection, Type elementType)
    {
        var query = (IQueryable)IgnoreNamedFiltersMethod
            .MakeGenericMethod(elementType)
            .Invoke(null, [collection.Query(), new[] { Configurations.QueryFilterNames.Moderation }])!;

        return query.Cast<object>().ToList();
    }

    public async Task<bool> DeleteListingAsync(
        ListingModuleType type, Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await FindForModerationAsync(type, id, cancellationToken);

        if (listing is null)
            return false;

        var entry = _context.Entry(listing);

        var isDeleted = entry.Properties
            .FirstOrDefault(property =>
                property.Metadata.Name == "IsDeleted" &&
                property.Metadata.ClrType == typeof(bool));

        if (isDeleted is null)
        {
            entry.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
        else
        {
            isDeleted.CurrentValue = true;

            var deletedAt = entry.Properties.FirstOrDefault(property =>
                property.Metadata.Name == "DeletedAt" &&
                (property.Metadata.ClrType == typeof(DateTime?) ||
                 property.Metadata.ClrType == typeof(DateTime)));

            if (deletedAt is not null)
                deletedAt.CurrentValue = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private IQueryable<UserListingRow>? BuildQuery(
        AdminAdFilterParams filter,
        string? search,
        IReadOnlyList<string> matchedUserIds,
        DateTime utcNow)
    {
        IQueryable<UserListingRow>? combined = null;

        foreach (var source in _sources)
        {
            if (!Participates(source.Type, filter))
                continue;

            var query = source.Query(filter.OwnerId, utcNow, includeUnmoderated: true);

            if (source.Type == ListingModuleType.Advertisement)
            {
                if (filter.SubCategoryId is { } subCategoryId)
                    query = query.Where(row => row.SubCategoryId == subCategoryId);
                else if (filter.CategoryId is { } categoryId)
                    query = query.Where(row => row.CategoryId == categoryId);
            }

            if (filter.ModerationStatus is { } moderationStatus)
                query = query.Where(row => row.ModerationStatus == moderationStatus);

            if (filter.Status is { } status)
                query = query.Where(row => row.Status == status);

            if (filter.FromDate is { } fromDate)
                query = query.Where(row => row.CreatedAt >= fromDate);

            if (filter.ToDate is { } toDate)
                query = query.Where(row => row.CreatedAt <= toDate);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(row =>
                    EF.Functions.Like(row.Title, $"%{search}%") ||
                    matchedUserIds.Contains(row.OwnerId));

            combined = combined is null ? query : combined.Concat(query);
        }

        return combined;
    }

    private static bool Participates(ListingModuleType type, AdminAdFilterParams filter)
    {
        if (filter.Type is { } requested && type != requested)
            return false;

        if (type == ListingModuleType.Advertisement)
            return (filter.SubCategoryId is null ||
                    ListingModuleCatalog.ModuleOf(filter.SubCategoryId.Value) == type) &&
                   (filter.CategoryId is null || filter.CategoryId == (int)CategoryType.Cars);

        if (filter.SubCategoryId is { } subCategoryId &&
            (int?)ListingModuleCatalog.SubCategoryOf(type) != subCategoryId)
            return false;

        if (filter.CategoryId is { } categoryId &&
            (int?)ListingModuleCatalog.CategoryOf(type) != categoryId)
            return false;

        return true;
    }
}
