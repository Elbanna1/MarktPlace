using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.HomeFurnishing;

namespace Persistence.Repositories;

public class BathroomSupplyRepository : IBathroomSupplyRepository
{
    private readonly AppDbContext _context;

    public BathroomSupplyRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<BathroomSupply?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<BathroomSupply> query = _context.BathroomSupplies
            .Include(x => x.Images)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<BathroomSupply?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.BathroomSupplies
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<BathroomSupply> Items, int TotalCount)> GetPagedAsync(
        BathroomSupplyFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.BathroomSupplies.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<BathroomSupply>(), 0);

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

    public async Task<IReadOnlyList<BathroomSupply>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.BathroomSupplies
            .AsNoTracking()
            .Select(x => new { x.Id, x.ProductType, x.Price })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<BathroomSupply>();

        return await PagedListingQuery.ToStripAsync(
            _context.BathroomSupplies
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

    public async Task<IReadOnlyList<BathroomSupply>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.BathroomSupplies
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.Material })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<BathroomSupply>();

        return await PagedListingQuery.ToStripAsync(
            _context.BathroomSupplies
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

    public async Task<IReadOnlyList<BathroomSupply>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await _context.BathroomSupplies
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    public async Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        BathroomSupplyFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.BathroomSupplies.AsNoTracking(), filter);

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
        await _context.BathroomSupplies
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
        _context.BathroomSupplies
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);

    public Task<List<BathroomSupplyProductTypeLookup>> GetProductTypesAsync(CancellationToken cancellationToken = default) =>
        _context.BathroomSupplyProductTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<BathroomSupplyMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default) =>
        _context.BathroomSupplyMaterials.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<BathroomSupplyColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.BathroomSupplyColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(BathroomSupply entity) => await _context.BathroomSupplies.AddAsync(entity);

    public void Update(BathroomSupply entity) => _context.BathroomSupplies.Update(entity);

    public void RemoveImage(BathroomSupplyImage image) => _context.BathroomSupplyImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<BathroomSupplyImage> images) =>
        await _context.BathroomSupplyImages.AddRangeAsync(images);

    public void RemoveColors(IEnumerable<BathroomSupplyColorSelection> colors) =>
        _context.BathroomSupplyColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<BathroomSupplyColorSelection> colors) =>
        await _context.BathroomSupplyColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<BathroomSupply> Filter(
        IQueryable<BathroomSupply> query, BathroomSupplyFilterParams filter)
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
