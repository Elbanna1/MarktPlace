using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Persistence.Repositories;

public class ShopRepository : IShopRepository
{
    private readonly AppDbContext _context;

    public ShopRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Shop?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Shop> query = _context.Shops
            .Include(x => x.Images)
            .Include(x => x.Utilities)
            .Include(x => x.RentInclusions)
            .Include(x => x.RentSuitableActivities);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Shop?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Shops
            .Include(x => x.Images)
            .Include(x => x.Utilities)
            .Include(x => x.RentInclusions)
            .Include(x => x.RentSuitableActivities)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Shop> Items, int TotalCount)> GetPagedAsync(
        ShopFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Shops.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Shop>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            RealEstateQueries.ApplyOrdering(query, filter.SortBy, nameof(Shop.Price)),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .Include(x => x.Utilities)
                .Include(x => x.RentInclusions)
                .Include(x => x.RentSuitableActivities)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Shop>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Shops
            .AsNoTracking()
            .Select(x => new { x.Id, x.SuitableActivity, x.Price })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Shop>();

        return await PagedListingQuery.ToStripAsync(
            _context.Shops
                .AsNoTracking()
                .Where(x => x.Id != id && x.SuitableActivity == listing.SuitableActivity)
                .OrderBy(x => x.Price > listing.Price ? x.Price - listing.Price : listing.Price - x.Price)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Utilities)
                .Include(x => x.RentInclusions)
                .Include(x => x.RentSuitableActivities)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<Shop>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Shops
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.Center })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Shop>();

        return await PagedListingQuery.ToStripAsync(
            _context.Shops
                .AsNoTracking()
                .Where(x => x.Id != id && (x.UserId == listing.UserId || x.Center == listing.Center))
                .OrderByDescending(x => x.UserId == listing.UserId)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Utilities)
                .Include(x => x.RentInclusions)
                .Include(x => x.RentSuitableActivities)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<Shop>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await _context.Shops
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Include(x => x.Images)
            .Include(x => x.Utilities)
            .Include(x => x.RentInclusions)
            .Include(x => x.RentSuitableActivities)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    public async Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        ShopFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Shops.AsNoTracking(), filter);

        var statistics = await query
            .GroupBy(_ => 1)
            .Select(group => new RealEstatePriceStatisticsDto
            {
                Count = group.Count(),
                MinPrice = group.Min(x => (decimal?)x.Price),
                MaxPrice = group.Max(x => (decimal?)x.Price),
                AveragePrice = group.Average(x => (decimal?)x.Price)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return statistics ?? new RealEstatePriceStatisticsDto();
    }

    public async Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default) =>
        await _context.Shops
            .AsNoTracking()
            .Where(x => EF.Functions.Like(x.Title, $"{term}%"))
            .GroupBy(x => x.Title)
            .Select(group => new RealEstateSuggestionDto
            {
                Term = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(suggestion => suggestion.Count)
            .ThenBy(suggestion => suggestion.Term)
            .Take(count)
            .ToListAsync(cancellationToken);

    public Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Shops
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);

    public Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync<TLookup>(
        CancellationToken cancellationToken = default)
        where TLookup : class, IRealEstateLookup =>
        RealEstateLookupReader.ReadAsync<TLookup>(_context, cancellationToken);

    public async Task AddAsync(Shop entity) => await _context.Shops.AddAsync(entity);

    public void Update(Shop entity) => _context.Shops.Update(entity);

    public void RemoveImage(ShopImage image) => _context.ShopImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<ShopImage> images) =>
        await _context.ShopImages.AddRangeAsync(images);

    public void RemoveUtilities(IEnumerable<ShopUtilitySelection> utilities) =>
        _context.ShopUtilitySelections.RemoveRange(utilities);

    public async Task AddUtilitiesAsync(IEnumerable<ShopUtilitySelection> utilities) =>
        await _context.ShopUtilitySelections.AddRangeAsync(utilities);

    public void RemoveRentInclusions(IEnumerable<ShopRentInclusionSelection> inclusions) =>
        _context.ShopRentInclusionSelections.RemoveRange(inclusions);

    public async Task AddRentInclusionsAsync(IEnumerable<ShopRentInclusionSelection> inclusions) =>
        await _context.ShopRentInclusionSelections.AddRangeAsync(inclusions);

    public void RemoveRentSuitableActivities(IEnumerable<ShopRentSuitableActivitySelection> activities) =>
        _context.ShopRentSuitableActivitySelections.RemoveRange(activities);

    public async Task AddRentSuitableActivitiesAsync(
        IEnumerable<ShopRentSuitableActivitySelection> activities) =>
        await _context.ShopRentSuitableActivitySelections.AddRangeAsync(activities);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<Shop> Filter(IQueryable<Shop> query, ShopFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                (x.District != null && EF.Functions.Like(x.District, $"%{search}%")) ||
                (x.Address != null && EF.Functions.Like(x.Address, $"%{search}%")) ||
                (x.OtherSuitableActivity != null && EF.Functions.Like(x.OtherSuitableActivity, $"%{search}%")) ||
                (x.PreviousActivity != null && EF.Functions.Like(x.PreviousActivity, $"%{search}%")) ||
                (x.OtherProject != null && EF.Functions.Like(x.OtherProject, $"%{search}%")));
        }

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        if (filter.SuitableActivity is { } suitableActivity)
            query = query.Where(x => x.SuitableActivity == suitableActivity);

        if (filter.AreaFrom is { } areaFrom)
            query = query.Where(x => x.Area >= areaFrom);

        if (filter.AreaTo is { } areaTo)
            query = query.Where(x => x.Area <= areaTo);

        if (filter.FloorType is { } floorType)
            query = query.Where(x => x.FloorType == floorType);

        if (filter.FinishingType is { } finishingType)
            query = query.Where(x => x.FinishingType == finishingType);

        if (filter.FacadesCount is { } facadesCount)
            query = query.Where(x => x.FacadesCount == facadesCount);

        if (filter.HasStorage is { } hasStorage)
            query = query.Where(x => x.HasStorage == hasStorage);

        if (filter.HasBathroom is { } hasBathroom)
            query = query.Where(x => x.HasBathroom == hasBathroom);

        if (filter.IsLicensed is { } isLicensed)
        {
            query = isLicensed
                ? query.Where(x => x.LegalStatus == ShopLegalStatus.Licensed)
                : query.Where(x => x.LegalStatus != ShopLegalStatus.Licensed);
        }

        if (filter.IsReconciliation is { } isReconciliation)
        {
            query = isReconciliation
                ? query.Where(x => x.LegalStatus == ShopLegalStatus.Reconciliation)
                : query.Where(x => x.LegalStatus != ShopLegalStatus.Reconciliation);
        }

        if (filter.LicenseType is { } licenseType)
            query = query.Where(x => x.LicenseType == licenseType);

        if (filter.OwnershipDocument is { } ownershipDocument)
            query = query.Where(x => x.OwnershipDocument == ownershipDocument);

        query = ApplyUtilityFilter(query, filter.HasParking, ShopUtility.Parking);
        query = ApplyUtilityFilter(query, filter.HasAirConditioning, ShopUtility.AirConditioning);
        query = ApplyUtilityFilter(query, filter.HasNaturalGas, ShopUtility.NaturalGas);
        query = ApplyUtilityFilter(query, filter.HasSurveillanceCameras, ShopUtility.SurveillanceCameras);

        return RealEstateQueries.ApplyCommonFilters(query, filter);
    }

    private static IQueryable<Shop> ApplyUtilityFilter(
        IQueryable<Shop> query, bool? wanted, ShopUtility utility) =>
        wanted switch
        {
            true => query.Where(x => x.Utilities.Any(selection => selection.Utility == utility)),
            false => query.Where(x => !x.Utilities.Any(selection => selection.Utility == utility)),
            _ => query
        };
}
