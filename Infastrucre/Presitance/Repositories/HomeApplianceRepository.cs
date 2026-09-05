using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.HomeFurnishing;

namespace Persistence.Repositories;

public class HomeApplianceRepository : IHomeApplianceRepository
{
    private readonly AppDbContext _context;

    public HomeApplianceRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<HomeAppliance?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<HomeAppliance> query = _context.HomeAppliances
            .Include(x => x.Images)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<HomeAppliance?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.HomeAppliances
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<HomeAppliance> Items, int TotalCount)> GetPagedAsync(
        HomeApplianceFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.HomeAppliances.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<HomeAppliance>(), 0);

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

    public async Task<IReadOnlyList<HomeAppliance>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.HomeAppliances
            .AsNoTracking()
            .Select(x => new { x.Id, x.DeviceType, x.Price })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<HomeAppliance>();

        return await PagedListingQuery.ToStripAsync(
            _context.HomeAppliances
                .AsNoTracking()
                .Where(x => x.Id != id && x.DeviceType == listing.DeviceType)
                .OrderBy(x => x.Price > listing.Price ? x.Price - listing.Price : listing.Price - x.Price)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Colors)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<HomeAppliance>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.HomeAppliances
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.Brand })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<HomeAppliance>();

        return await PagedListingQuery.ToStripAsync(
            _context.HomeAppliances
                .AsNoTracking()
                .Where(x => x.Id != id && (x.UserId == listing.UserId || x.Brand == listing.Brand))
                .OrderByDescending(x => x.UserId == listing.UserId)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Colors)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<HomeAppliance>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await _context.HomeAppliances
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    public async Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        HomeApplianceFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.HomeAppliances.AsNoTracking(), filter);

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
        await _context.HomeAppliances
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
        _context.HomeAppliances
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);

    public Task<List<HomeApplianceDeviceTypeLookup>> GetDeviceTypesAsync(CancellationToken cancellationToken = default) =>
        _context.HomeApplianceDeviceTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HomeApplianceBrandLookup>> GetBrandsAsync(CancellationToken cancellationToken = default) =>
        _context.HomeApplianceBrands.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HomeApplianceConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default) =>
        _context.HomeApplianceConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HomeApplianceWarrantyLookup>> GetWarrantiesAsync(CancellationToken cancellationToken = default) =>
        _context.HomeApplianceWarranties.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HomeApplianceColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.HomeApplianceColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(HomeAppliance entity) => await _context.HomeAppliances.AddAsync(entity);

    public void Update(HomeAppliance entity) => _context.HomeAppliances.Update(entity);

    public void RemoveImage(HomeApplianceImage image) => _context.HomeApplianceImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<HomeApplianceImage> images) =>
        await _context.HomeApplianceImages.AddRangeAsync(images);

    public void RemoveColors(IEnumerable<HomeApplianceColorSelection> colors) =>
        _context.HomeApplianceColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<HomeApplianceColorSelection> colors) =>
        await _context.HomeApplianceColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<HomeAppliance> Filter(
        IQueryable<HomeAppliance> query, HomeApplianceFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.ProductName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                (x.OtherDeviceType != null && EF.Functions.Like(x.OtherDeviceType, $"%{search}%")) ||
                (x.OtherBrand != null && EF.Functions.Like(x.OtherBrand, $"%{search}%")));
        }

        if (filter.DeviceType is { } deviceType)
            query = query.Where(x => x.DeviceType == deviceType);

        if (filter.Brand is { } brand)
            query = query.Where(x => x.Brand == brand);

        if (filter.Color is { } color)
            query = query.Where(x => x.Colors.Any(selection => selection.Color == color));

        if (filter.Condition is { } condition)
            query = query.Where(x => x.Condition == condition);

        if (filter.Warranty is { } warranty)
            query = query.Where(x => x.Warranty == warranty);

        if (filter.DeliveryAvailable is { } deliveryAvailable)
            query = query.Where(x => x.DeliveryAvailable == deliveryAvailable);

        return HomeFurnishingQueries.ApplyCommonFilters(query, filter);
    }
}
