using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.HomeFurnishing;
using Shared.Enums;

namespace Persistence.Repositories;

public class FurnitureRepository : IFurnitureRepository
{
    private readonly AppDbContext _context;

    public FurnitureRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Furniture?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Furniture> query = _context.Furnitures
            .Include(x => x.Images)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Furniture?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Furnitures
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Furniture> Items, int TotalCount)> GetPagedAsync(
        FurnitureFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Furnitures.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Furniture>(), 0);

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

    public async Task<IReadOnlyList<Furniture>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Furnitures
            .AsNoTracking()
            .Select(x => new { x.Id, x.FurnitureType, x.Price })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Furniture>();

        return await PagedListingQuery.ToStripAsync(
            _context.Furnitures
                .AsNoTracking()
                .Where(x => x.Id != id && x.FurnitureType == listing.FurnitureType)
                .OrderBy(x => x.Price > listing.Price ? x.Price - listing.Price : listing.Price - x.Price)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Colors)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<Furniture>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Furnitures
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.Material })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Furniture>();

        return await PagedListingQuery.ToStripAsync(
            _context.Furnitures
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

    public async Task<IReadOnlyList<Furniture>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await _context.Furnitures
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    public async Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        FurnitureFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Furnitures.AsNoTracking(), filter);

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
        await _context.Furnitures
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

        _context.Furnitures
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);

    public Task<List<FurnitureTypeLookup>> GetFurnitureTypesAsync(CancellationToken cancellationToken = default) =>
        _context.FurnitureTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<FurnitureMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default) =>
        _context.FurnitureMaterials.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<FurnitureColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.FurnitureColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<FurnitureConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default) =>
        _context.FurnitureConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Furniture entity) => await _context.Furnitures.AddAsync(entity);

    public void Update(Furniture entity) => _context.Furnitures.Update(entity);

    public void RemoveImage(FurnitureImage image) => _context.FurnitureImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<FurnitureImage> images) =>
        await _context.FurnitureImages.AddRangeAsync(images);

    public void RemoveColors(IEnumerable<FurnitureColorSelection> colors) =>
        _context.FurnitureColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<FurnitureColorSelection> colors) =>
        await _context.FurnitureColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<Furniture> Filter(IQueryable<Furniture> query, FurnitureFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.ProductName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                (x.OtherFurnitureType != null && EF.Functions.Like(x.OtherFurnitureType, $"%{search}%")) ||
                (x.OtherMaterial != null && EF.Functions.Like(x.OtherMaterial, $"%{search}%")));
        }

        if (filter.FurnitureType is { } furnitureType)
            query = query.Where(x => x.FurnitureType == furnitureType);

        if (filter.Material is { } material)
            query = query.Where(x => x.Material == material);

        if (filter.Color is { } color)
            query = query.Where(x => x.Colors.Any(selection => selection.Color == color));

        if (filter.Condition is { } condition)
            query = query.Where(x => x.Condition == condition);

        if (filter.DeliveryAvailable is { } deliveryAvailable)
            query = query.Where(x => x.DeliveryAvailable == deliveryAvailable);

        if (filter.CanBeDisassembled is { } canBeDisassembled)
            query = query.Where(x => x.CanBeDisassembled == canBeDisassembled);

        return HomeFurnishingQueries.ApplyCommonFilters(query, filter);
    }
}
