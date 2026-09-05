using Domain.Entities.Listings;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Listings;
using Shared.Enums;

namespace Persistence.Repositories;

public class ListingInteractionRepository : IListingInteractionRepository
{
    private readonly AppDbContext _context;

    public ListingInteractionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<ListingViewer?> GetViewerAsync(
        ListingModuleType type, Guid listingId, string viewerKey, CancellationToken cancellationToken = default) =>
        _context.ListingViewers
            .FirstOrDefaultAsync(
                v => v.ListingType == type && v.ListingId == listingId && v.ViewerKey == viewerKey,
                cancellationToken);

    public Task<ListingViewCounter?> GetCounterAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default) =>
        _context.ListingViewCounters
            .FirstOrDefaultAsync(c => c.ListingType == type && c.ListingId == listingId, cancellationToken);

    public void AddViewer(ListingViewer viewer) => _context.ListingViewers.Add(viewer);

    public void AddCounter(ListingViewCounter counter) => _context.ListingViewCounters.Add(counter);

    public Task<int> IncrementViewsAsync(
        ListingModuleType type, Guid listingId, int delta, DateTime viewedAt,
        CancellationToken cancellationToken = default) =>

        _context.ListingViewCounters
            .Where(c => c.ListingType == type && c.ListingId == listingId)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(c => c.TotalViews, c => c.TotalViews + delta)
                    .SetProperty(c => c.LastViewedAt, viewedAt),
                cancellationToken);

    public async Task TrimRecentlyViewedAsync(
        string userId, int keep, CancellationToken cancellationToken = default)
    {
        var cutoff = await _context.ListingViewers
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.LastViewedAt)
            .Select(v => v.LastViewedAt)
            .Skip(keep)
            .FirstOrDefaultAsync(cancellationToken);

        if (cutoff == default)
            return;

        await _context.ListingViewers
            .Where(v => v.UserId == userId && v.LastViewedAt <= cutoff)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<(ListingModuleType Type, Guid ListingId, DateTime LastViewedAt)> Items, int TotalCount)>
        GetRecentlyViewedAsync(
            string userId, ListingModuleType? type, int pageIndex, int pageSize,
            CancellationToken cancellationToken = default)
    {
        var query = _context.ListingViewers
            .AsNoTracking()
            .Where(v => v.UserId == userId);

        if (type is { } requested)
            query = query.Where(v => v.ListingType == requested);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<(ListingModuleType, Guid, DateTime)>(), 0);

        var rows = await query
            .OrderByDescending(v => v.LastViewedAt)
            .ThenByDescending(v => v.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(v => new { v.ListingType, v.ListingId, v.LastViewedAt })
            .ToListAsync(cancellationToken);

        return (rows.Select(r => (r.ListingType, r.ListingId, r.LastViewedAt)).ToList(), totalCount);
    }

    public Task<int> ClearRecentlyViewedAsync(
        string userId, ListingModuleType? type, CancellationToken cancellationToken = default)
    {
        var query = _context.ListingViewers.Where(v => v.UserId == userId);

        if (type is { } requested)
            query = query.Where(v => v.ListingType == requested);

        return query.ExecuteDeleteAsync(cancellationToken);
    }

    public Task<int> RemoveRecentlyViewedAsync(
        string userId, ListingModuleType type, Guid listingId,
        CancellationToken cancellationToken = default) =>
        _context.ListingViewers
            .Where(v => v.UserId == userId && v.ListingType == type && v.ListingId == listingId)
            .ExecuteDeleteAsync(cancellationToken);

    public Task<ListingFavorite?> GetFavoriteAsync(
        string userId, ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default) =>
        _context.ListingFavorites
            .FirstOrDefaultAsync(
                f => f.UserId == userId && f.ListingType == type && f.ListingId == listingId,
                cancellationToken);

    public void AddFavorite(ListingFavorite favorite) => _context.ListingFavorites.Add(favorite);

    public void RemoveFavorite(ListingFavorite favorite) => _context.ListingFavorites.Remove(favorite);

    public Task<int> CountFavoritesAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default) =>
        _context.ListingFavorites
            .CountAsync(f => f.ListingType == type && f.ListingId == listingId, cancellationToken);

    public async Task<(IReadOnlyList<(ListingModuleType Type, Guid ListingId, DateTime CreatedAt)> Items, int TotalCount)>
        GetFavoritesAsync(
            string userId, ListingModuleType? type, int pageIndex, int pageSize,
            CancellationToken cancellationToken = default)
    {
        var query = _context.ListingFavorites
            .AsNoTracking()
            .Where(f => f.UserId == userId);

        if (type is { } requested)
            query = query.Where(f => f.ListingType == requested);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<(ListingModuleType, Guid, DateTime)>(), 0);

        var rows = await query
            .OrderByDescending(f => f.CreatedAt)
            .ThenByDescending(f => f.ListingId)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(f => new { f.ListingType, f.ListingId, f.CreatedAt })
            .ToListAsync(cancellationToken);

        return (rows.Select(r => (r.ListingType, r.ListingId, r.CreatedAt)).ToList(), totalCount);
    }

    public async Task<IReadOnlyDictionary<(ListingModuleType Type, Guid ListingId), ListingCountersDto>>
        GetCountersAsync(
            IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
            string? userId,
            CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<(ListingModuleType, Guid), ListingCountersDto>();

        if (keys.Count == 0)
            return result;

        foreach (var key in keys)
            result[key] = new ListingCountersDto();

        var ids = keys.Select(k => k.ListingId).Distinct().ToList();
        var types = keys.Select(k => k.Type).Distinct().ToList();
        var wanted = keys.ToHashSet();

        var views = await _context.ListingViewCounters
            .AsNoTracking()
            .Where(c => types.Contains(c.ListingType) && ids.Contains(c.ListingId))
            .Select(c => new { c.ListingType, c.ListingId, c.TotalViews })
            .ToListAsync(cancellationToken);

        foreach (var row in views)
        {
            var key = (row.ListingType, row.ListingId);
            if (wanted.Contains(key))
                result[key].Views = row.TotalViews;
        }

        var favoriteCounts = await _context.ListingFavorites
            .AsNoTracking()
            .Where(f => types.Contains(f.ListingType) && ids.Contains(f.ListingId))
            .GroupBy(f => new { f.ListingType, f.ListingId })
            .Select(g => new { g.Key.ListingType, g.Key.ListingId, Count = g.Count() })
            .ToListAsync(cancellationToken);

        foreach (var row in favoriteCounts)
        {
            var key = (row.ListingType, row.ListingId);
            if (wanted.Contains(key))
                result[key].FavoriteCount = row.Count;
        }

        var ratings = await _context.ListingRatings
            .AsNoTracking()
            .Where(r => types.Contains(r.ListingType) && ids.Contains(r.ListingId))
            .GroupBy(r => new { r.ListingType, r.ListingId })
            .Select(g => new
            {
                g.Key.ListingType,
                g.Key.ListingId,
                Count = g.Count(),
                Average = g.Average(r => (double)r.Rating)
            })
            .ToListAsync(cancellationToken);

        foreach (var row in ratings)
        {
            var key = (row.ListingType, row.ListingId);

            if (!wanted.Contains(key))
                continue;

            result[key].RatingsCount = row.Count;
            result[key].AverageRating = Math.Round((decimal)row.Average, 1, MidpointRounding.AwayFromZero);
        }

        if (!string.IsNullOrEmpty(userId))
        {
            var mine = await _context.ListingFavorites
                .AsNoTracking()
                .Where(f => f.UserId == userId && types.Contains(f.ListingType) && ids.Contains(f.ListingId))
                .Select(f => new { f.ListingType, f.ListingId })
                .ToListAsync(cancellationToken);

            foreach (var row in mine)
            {
                var key = (row.ListingType, row.ListingId);
                if (wanted.Contains(key))
                    result[key].IsFavorite = true;
            }

            var myRatings = await _context.ListingRatings
                .AsNoTracking()
                .Where(r => r.ReviewerUserId == userId && types.Contains(r.ListingType) && ids.Contains(r.ListingId))
                .Select(r => new { r.ListingType, r.ListingId, r.Rating })
                .ToListAsync(cancellationToken);

            foreach (var row in myRatings)
            {
                var key = (row.ListingType, row.ListingId);
                if (wanted.Contains(key))
                    result[key].MyRating = row.Rating;
            }
        }

        return result;
    }

    public async Task<(int TotalViews, int TotalFavorites)> GetOwnerTotalsAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> ownedKeys,
        CancellationToken cancellationToken = default)
    {
        if (ownedKeys.Count == 0)
            return (0, 0);

        var ids = ownedKeys.Select(k => k.ListingId).Distinct().ToList();
        var types = ownedKeys.Select(k => k.Type).Distinct().ToList();
        var wanted = ownedKeys.ToHashSet();

        var views = await _context.ListingViewCounters
            .AsNoTracking()
            .Where(c => types.Contains(c.ListingType) && ids.Contains(c.ListingId))
            .Select(c => new { c.ListingType, c.ListingId, c.TotalViews })
            .ToListAsync(cancellationToken);

        var favorites = await _context.ListingFavorites
            .AsNoTracking()
            .Where(f => types.Contains(f.ListingType) && ids.Contains(f.ListingId))
            .Select(f => new { f.ListingType, f.ListingId })
            .ToListAsync(cancellationToken);

        return (
            views.Where(v => wanted.Contains((v.ListingType, v.ListingId))).Sum(v => v.TotalViews),
            favorites.Count(f => wanted.Contains((f.ListingType, f.ListingId))));
    }

    public Task<ListingReport?> GetReportAsync(
        ListingModuleType type, Guid listingId, string reporterUserId,
        CancellationToken cancellationToken = default) =>
        _context.ListingReports
            .FirstOrDefaultAsync(
                r => r.ListingType == type && r.ListingId == listingId && r.ReporterUserId == reporterUserId,
                cancellationToken);

    public Task<ListingReport?> GetReportByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.ListingReports.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public void AddReport(ListingReport report) => _context.ListingReports.Add(report);

    public async Task<(IReadOnlyList<ListingReport> Items, int TotalCount)> GetReportsAsync(
        ListingReportFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.ListingReports.AsNoTracking().AsQueryable();

        if (filter.Status is { } status)
            query = query.Where(r => r.Status == status);

        if (filter.Reason is { } reason)
            query = query.Where(r => r.Reason == reason);

        if (filter.Type is { } type)
            query = query.Where(r => r.ListingType == type);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<ListingReport>(), 0);

        var items = await query
            .Include(r => r.Reporter)
            .OrderByDescending(r => r.CreatedAt)
            .ThenByDescending(r => r.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<ListingRating?> GetRatingAsync(
        ListingModuleType type, Guid listingId, string reviewerUserId,
        CancellationToken cancellationToken = default) =>

        _context.ListingRatings
            .FirstOrDefaultAsync(
                r => r.ListingType == type && r.ListingId == listingId &&
                     r.ReviewerUserId == reviewerUserId,
                cancellationToken);

    public void AddRating(ListingRating rating) => _context.ListingRatings.Add(rating);

    public void RemoveRating(ListingRating rating) => _context.ListingRatings.Remove(rating);

    public async Task<ListingRatingSummaryDto> GetRatingSummaryAsync(
        ListingModuleType type, Guid listingId, string? viewerUserId,
        CancellationToken cancellationToken = default)
    {
        var perStar = await _context.ListingRatings
            .AsNoTracking()
            .Where(r => r.ListingType == type && r.ListingId == listingId)
            .GroupBy(r => r.Rating)
            .Select(g => new { Stars = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var breakdown = Enumerable
            .Range(ListingInteractionCatalog.MinRating, ListingInteractionCatalog.MaxRating)
            .ToDictionary(stars => stars, stars => perStar.FirstOrDefault(p => p.Stars == stars)?.Count ?? 0);

        var count = perStar.Sum(p => p.Count);

        var summary = new ListingRatingSummaryDto
        {
            ListingType = type,
            ListingId = listingId,
            RatingsCount = count,
            AverageRating = count == 0
                ? null
                : Math.Round(
                    (decimal)perStar.Sum(p => (long)p.Stars * p.Count) / count, 1,
                    MidpointRounding.AwayFromZero),
            Breakdown = breakdown
        };

        if (!string.IsNullOrEmpty(viewerUserId))
        {
            summary.MyRating = await _context.ListingRatings
                .AsNoTracking()
                .Where(r => r.ListingType == type && r.ListingId == listingId &&
                            r.ReviewerUserId == viewerUserId)
                .Select(r => (int?)r.Rating)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return summary;
    }

    public async Task<(IReadOnlyList<ListingRating> Items, int TotalCount)> GetRatingsAsync(
        ListingRatingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.ListingRatings.AsNoTracking().AsQueryable();

        if (filter.ListingType is { } type)
            query = query.Where(r => r.ListingType == type);

        if (filter.ListingId is { } listingId)
            query = query.Where(r => r.ListingId == listingId);

        if (!string.IsNullOrEmpty(filter.ReviewerUserId))
            query = query.Where(r => r.ReviewerUserId == filter.ReviewerUserId);

        if (filter.Rating is { } exact)
            query = query.Where(r => r.Rating == exact);

        if (filter.MinRating is { } min)
            query = query.Where(r => r.Rating >= min);

        if (filter.MaxRating is { } max)
            query = query.Where(r => r.Rating <= max);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<ListingRating>(), 0);

        var items = await query
            .Include(r => r.Reviewer)
            .OrderByDescending(r => r.UpdatedAt ?? r.CreatedAt)
            .ThenByDescending(r => r.Id)
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<ListingRating?> GetRatingByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.ListingRatings
            .Include(r => r.Reviewer)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task PurgeListingAsync(
        ListingModuleType type, Guid listingId, CancellationToken cancellationToken = default)
    {
        await _context.ListingViewers
            .Where(v => v.ListingType == type && v.ListingId == listingId)
            .ExecuteDeleteAsync(cancellationToken);

        await _context.ListingViewCounters
            .Where(c => c.ListingType == type && c.ListingId == listingId)
            .ExecuteDeleteAsync(cancellationToken);

        await _context.ListingFavorites
            .Where(f => f.ListingType == type && f.ListingId == listingId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ListingReport>> CloseOpenReportsForListingAsync(
        ListingModuleType type, Guid listingId, string adminNote, DateTime reviewedAt,
        CancellationToken cancellationToken = default)
    {
        var open = await _context.ListingReports
            .Where(r => r.ListingType == type
                        && r.ListingId == listingId
                        && (r.Status == ListingReportStatus.Pending
                            || r.Status == ListingReportStatus.UnderReview))
            .ToListAsync(cancellationToken);

        if (open.Count == 0)
            return Array.Empty<ListingReport>();

        foreach (var report in open)
        {
            report.Status = ListingReportStatus.ActionTaken;
            report.ReviewedAt = reviewedAt;

            report.AdminNote = string.IsNullOrWhiteSpace(report.AdminNote)
                ? adminNote
                : $"{report.AdminNote}{Environment.NewLine}{adminNote}";
        }

        await _context.SaveChangesAsync(cancellationToken);

        return open;
    }

    public async Task<int> PurgeOrphanInteractionsAsync(
        IReadOnlyCollection<(ListingModuleType Type, Guid ListingId)> keys,
        CancellationToken cancellationToken = default)
    {
        if (keys.Count == 0)
            return 0;

        var removed = 0;

        foreach (var group in keys.GroupBy(key => key.Type))
        {
            var type = group.Key;
            var ids = group.Select(key => key.ListingId).Distinct().ToList();

            removed += await _context.ListingViewers
                .Where(v => v.ListingType == type && ids.Contains(v.ListingId))
                .ExecuteDeleteAsync(cancellationToken);

            removed += await _context.ListingFavorites
                .Where(f => f.ListingType == type && ids.Contains(f.ListingId))
                .ExecuteDeleteAsync(cancellationToken);

            await _context.ListingViewCounters
                .Where(c => c.ListingType == type && ids.Contains(c.ListingId))
                .ExecuteDeleteAsync(cancellationToken);
        }

        return removed;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
