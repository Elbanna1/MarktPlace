using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.HomeFurnishing;

namespace Persistence.Repositories;

public class PlantOrnamentRepository : IPlantOrnamentRepository
{
    private readonly AppDbContext _context;

    public PlantOrnamentRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<PlantOrnament?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<PlantOrnament> query = _context.PlantOrnaments.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<PlantOrnament?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.PlantOrnaments
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<PlantOrnament> Items, int TotalCount)> GetPagedAsync(
        PlantOrnamentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.PlantOrnaments.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<PlantOrnament>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            HomeFurnishingQueries.ApplyOrdering(query, filter.SortBy),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<PlantOrnament>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.PlantOrnaments
            .AsNoTracking()
            .Select(x => new { x.Id, x.ProductType, x.Price })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<PlantOrnament>();

        return await _context.PlantOrnaments
            .AsNoTracking()
            .Where(x => x.Id != id && x.ProductType == listing.ProductType)
            .OrderBy(x => x.Price > listing.Price ? x.Price - listing.Price : listing.Price - x.Price)
            .ThenByDescending(x => x.CreatedAt)
            .Take(count)
            .Include(x => x.Images)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PlantOrnament>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.PlantOrnaments
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.SuitableFor })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<PlantOrnament>();

        return await _context.PlantOrnaments
            .AsNoTracking()
            .Where(x => x.Id != id && (x.UserId == listing.UserId || x.SuitableFor == listing.SuitableFor))
            .OrderByDescending(x => x.UserId == listing.UserId)
            .ThenByDescending(x => x.CreatedAt)
            .Take(count)
            .Include(x => x.Images)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PlantOrnament>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await _context.PlantOrnaments
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Include(x => x.Images)
            .ToListAsync(cancellationToken);

    public async Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        PlantOrnamentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.PlantOrnaments.AsNoTracking(), filter);

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
        await _context.PlantOrnaments
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
        _context.PlantOrnaments
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);

    public Task<List<PlantOrnamentProductTypeLookup>> GetProductTypesAsync(CancellationToken cancellationToken = default) =>
        _context.PlantOrnamentProductTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PlantOrnamentSuitableForLookup>> GetSuitableForsAsync(CancellationToken cancellationToken = default) =>
        _context.PlantOrnamentSuitableFors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(PlantOrnament entity) => await _context.PlantOrnaments.AddAsync(entity);

    public void Update(PlantOrnament entity) => _context.PlantOrnaments.Update(entity);

    public void RemoveImage(PlantOrnamentImage image) => _context.PlantOrnamentImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<PlantOrnamentImage> images) =>
        await _context.PlantOrnamentImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<PlantOrnament> Filter(
        IQueryable<PlantOrnament> query, PlantOrnamentFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.ProductName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                (x.OtherProductType != null && EF.Functions.Like(x.OtherProductType, $"%{search}%")));
        }

        if (filter.ProductType is { } productType)
            query = query.Where(x => x.ProductType == productType);

        if (filter.SuitableFor is { } suitableFor)
            query = query.Where(x => x.SuitableFor == suitableFor);

        if (filter.HeightFrom is { } heightFrom)
            query = query.Where(x => x.Height >= heightFrom);

        if (filter.HeightTo is { } heightTo)
            query = query.Where(x => x.Height <= heightTo);

        return HomeFurnishingQueries.ApplyCommonFilters(query, filter);
    }
}
