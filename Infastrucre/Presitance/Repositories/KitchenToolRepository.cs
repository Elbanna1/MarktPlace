using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.HomeFurnishing;

namespace Persistence.Repositories;

public class KitchenToolRepository : IKitchenToolRepository
{
    private readonly AppDbContext _context;

    public KitchenToolRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<KitchenTool?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<KitchenTool> query = _context.KitchenTools
            .Include(x => x.Images)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<KitchenTool?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.KitchenTools
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<KitchenTool> Items, int TotalCount)> GetPagedAsync(
        KitchenToolFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.KitchenTools.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<KitchenTool>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            HomeFurnishingQueries.ApplyOrdering(query, filter.SortBy),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .Include(x => x.Colors)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<KitchenTool>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.KitchenTools
            .AsNoTracking()
            .Select(x => new { x.Id, x.ProductType, x.Price })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<KitchenTool>();

        return await PagedListingQuery.ToStripAsync(
            _context.KitchenTools
                .AsNoTracking()
                .Where(x => x.Id != id && x.ProductType == listing.ProductType)
                .OrderBy(x => x.Price > listing.Price ? x.Price - listing.Price : listing.Price - x.Price)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Colors)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<KitchenTool>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.KitchenTools
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.Material })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<KitchenTool>();

        return await PagedListingQuery.ToStripAsync(
            _context.KitchenTools
                .AsNoTracking()
                .Where(x => x.Id != id && (x.UserId == listing.UserId || x.Material == listing.Material))
                .OrderByDescending(x => x.UserId == listing.UserId)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Colors)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<KitchenTool>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await _context.KitchenTools
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    public async Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        KitchenToolFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.KitchenTools.AsNoTracking(), filter);

        var statistics = await query
            .GroupBy(_ => 1)
            .Select(group => new HomeFurnishingPriceStatisticsDto
            {
                Count = group.Count(),
                MinPrice = group.Min(x => (decimal?)x.Price),
                MaxPrice = group.Max(x => (decimal?)x.Price),
                AveragePrice = group.Average(x => (decimal?)x.Price)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return statistics ?? new HomeFurnishingPriceStatisticsDto();
    }

    public async Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default) =>
        await _context.KitchenTools
            .AsNoTracking()
            .Where(x => EF.Functions.Like(x.ProductName, $"{term}%"))
            .GroupBy(x => x.ProductName)
            .Select(group => new HomeFurnishingSuggestionDto
            {
                Term = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(suggestion => suggestion.Count)
            .ThenBy(suggestion => suggestion.Term)
            .Take(count)
            .ToListAsync(cancellationToken);

    public Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.KitchenTools
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);

    public Task<List<KitchenToolProductTypeLookup>> GetProductTypesAsync(CancellationToken cancellationToken = default) =>
        _context.KitchenToolProductTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<KitchenToolMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default) =>
        _context.KitchenToolMaterials.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<KitchenToolColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.KitchenToolColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(KitchenTool entity) => await _context.KitchenTools.AddAsync(entity);

    public void Update(KitchenTool entity) => _context.KitchenTools.Update(entity);

    public void RemoveImage(KitchenToolImage image) => _context.KitchenToolImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<KitchenToolImage> images) =>
        await _context.KitchenToolImages.AddRangeAsync(images);

    public void RemoveColors(IEnumerable<KitchenToolColorSelection> colors) =>
        _context.KitchenToolColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<KitchenToolColorSelection> colors) =>
        await _context.KitchenToolColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<KitchenTool> Filter(
        IQueryable<KitchenTool> query, KitchenToolFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.ProductName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                (x.OtherProductType != null && EF.Functions.Like(x.OtherProductType, $"%{search}%")) ||
                (x.OtherMaterial != null && EF.Functions.Like(x.OtherMaterial, $"%{search}%")));
        }

        if (filter.ProductType is { } productType)
            query = query.Where(x => x.ProductType == productType);

        if (filter.Material is { } material)
            query = query.Where(x => x.Material == material);

        if (filter.Color is { } color)
            query = query.Where(x => x.Colors.Any(selection => selection.Color == color));

        return HomeFurnishingQueries.ApplyCommonFilters(query, filter);
    }
}
