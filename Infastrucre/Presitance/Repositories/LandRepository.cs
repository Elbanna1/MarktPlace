using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.RealEstate;

namespace Persistence.Repositories;

public class LandRepository : ILandRepository
{
    private readonly AppDbContext _context;

    public LandRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Land?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Land> query = _context.Lands
            .Include(x => x.Images)
            .Include(x => x.Utilities)
            .Include(x => x.RentInclusions);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Land?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Lands
            .Include(x => x.Images)
            .Include(x => x.Utilities)
            .Include(x => x.RentInclusions)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Land> Items, int TotalCount)> GetPagedAsync(
        LandFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Lands.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Land>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            RealEstateQueries.ApplyOrdering(query, filter.SortBy, nameof(Land.TotalPrice)),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .Include(x => x.Utilities)
                .Include(x => x.RentInclusions)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Land>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Lands
            .AsNoTracking()
            .Select(x => new { x.Id, x.LandType, x.TotalPrice })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Land>();

        return await PagedListingQuery.ToStripAsync(
            _context.Lands
                .AsNoTracking()
                .Where(x => x.Id != id && x.LandType == listing.LandType)
                .OrderBy(x => x.TotalPrice > listing.TotalPrice
                    ? x.TotalPrice - listing.TotalPrice
                    : listing.TotalPrice - x.TotalPrice)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Utilities)
                .Include(x => x.RentInclusions)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<Land>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Lands
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.Center })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Land>();

        return await PagedListingQuery.ToStripAsync(
            _context.Lands
                .AsNoTracking()
                .Where(x => x.Id != id && (x.UserId == listing.UserId || x.Center == listing.Center))
                .OrderByDescending(x => x.UserId == listing.UserId)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Utilities)
                .Include(x => x.RentInclusions)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<Land>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await _context.Lands
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Include(x => x.Images)
            .Include(x => x.Utilities)
            .Include(x => x.RentInclusions)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    public async Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        LandFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Lands.AsNoTracking(), filter);

        var statistics = await query
            .GroupBy(_ => 1)
            .Select(group => new RealEstatePriceStatisticsDto
            {
                Count = group.Count(),
                MinPrice = group.Min(x => (decimal?)x.TotalPrice),
                MaxPrice = group.Max(x => (decimal?)x.TotalPrice),
                AveragePrice = group.Average(x => (decimal?)x.TotalPrice)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return statistics ?? new RealEstatePriceStatisticsDto();
    }

    public async Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default) =>
        await _context.Lands
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

        _context.Lands
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);

    public Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync<TLookup>(
        CancellationToken cancellationToken = default)
        where TLookup : class, IRealEstateLookup =>
        RealEstateLookupReader.ReadAsync<TLookup>(_context, cancellationToken);

    public async Task AddAsync(Land entity) => await _context.Lands.AddAsync(entity);

    public void Update(Land entity) => _context.Lands.Update(entity);

    public void RemoveImage(LandImage image) => _context.LandImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<LandImage> images) =>
        await _context.LandImages.AddRangeAsync(images);

    public void RemoveUtilities(IEnumerable<LandUtilitySelection> utilities) =>
        _context.LandUtilitySelections.RemoveRange(utilities);

    public async Task AddUtilitiesAsync(IEnumerable<LandUtilitySelection> utilities) =>
        await _context.LandUtilitySelections.AddRangeAsync(utilities);

    public void RemoveRentInclusions(IEnumerable<LandRentInclusionSelection> inclusions) =>
        _context.LandRentInclusionSelections.RemoveRange(inclusions);

    public async Task AddRentInclusionsAsync(IEnumerable<LandRentInclusionSelection> inclusions) =>
        await _context.LandRentInclusionSelections.AddRangeAsync(inclusions);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<Land> Filter(IQueryable<Land> query, LandFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                (x.District != null && EF.Functions.Like(x.District, $"%{search}%")) ||
                (x.Address != null && EF.Functions.Like(x.Address, $"%{search}%")) ||
                (x.OtherLandType != null && EF.Functions.Like(x.OtherLandType, $"%{search}%")) ||
                (x.OtherProject != null && EF.Functions.Like(x.OtherProject, $"%{search}%")));
        }

        if (filter.LandType is { } landType)
            query = query.Where(x => x.LandType == landType);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.TotalPrice >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.TotalPrice <= priceTo);

        if (filter.PricePerMeterFrom is { } pricePerMeterFrom)
            query = query.Where(x => x.PricePerMeter >= pricePerMeterFrom);

        if (filter.PricePerMeterTo is { } pricePerMeterTo)
            query = query.Where(x => x.PricePerMeter <= pricePerMeterTo);

        if (filter.AreaFrom is { } areaFrom)
            query = query.Where(x => x.Area >= areaFrom);

        if (filter.AreaTo is { } areaTo)
            query = query.Where(x => x.Area <= areaTo);

        if (filter.AreaUnit is { } areaUnit)
            query = query.Where(x => x.AreaUnit == areaUnit);

        if (filter.InsideBuildingCordon is { } insideBuildingCordon)
            query = query.Where(x => x.InsideBuildingCordon == insideBuildingCordon);

        if (filter.IsBuildable is { } isBuildable)
            query = query.Where(x => x.IsBuildable == isBuildable);

        if (filter.LegalStatus is { } legalStatus)
            query = query.Where(x => x.LegalStatus == legalStatus);

        if (filter.OwnershipDocument is { } ownershipDocument)
            query = query.Where(x => x.OwnershipDocument == ownershipDocument);

        if (filter.RoadType is { } roadType)
            query = query.Where(x => x.RoadType == roadType);

        if (filter.FacadesCount is { } facadesCount)
            query = query.Where(x => x.FacadesCount == facadesCount);

        if (filter.Direction is { } direction)
            query = query.Where(x => x.Direction == direction);

        if (filter.IsCurrentlyCultivated is { } isCurrentlyCultivated)
            query = query.Where(x => x.IsCurrentlyCultivated == isCurrentlyCultivated);

        if (filter.IrrigationSource is { } irrigationSource)
            query = query.Where(x => x.IrrigationSource == irrigationSource);

        if (filter.IsOrganic is { } isOrganic)
            query = query.Where(x => x.IsOrganic == isOrganic);

        if (filter.HasWell is { } hasWell)
            query = query.Where(x => x.HasWell == hasWell);

        if (filter.HasIrrigationNetwork is { } hasIrrigationNetwork)
            query = query.Where(x => x.HasIrrigationNetwork == hasIrrigationNetwork);

        if (filter.HasFence is { } hasFence)
            query = query.Where(x => x.HasFence == hasFence);

        return RealEstateQueries.ApplyCommonFilters(query, filter);
    }
}
