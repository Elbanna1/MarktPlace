using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Persistence.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly AppDbContext _context;

    public BuildingRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Building?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Building> query = _context.Buildings
            .Include(x => x.Images)
            .Include(x => x.Features)
            .Include(x => x.RentInclusions);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Building?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Buildings
            .Include(x => x.Images)
            .Include(x => x.Features)
            .Include(x => x.RentInclusions)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Building> Items, int TotalCount)> GetPagedAsync(
        BuildingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Buildings.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Building>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            RealEstateQueries.ApplyOrdering(query, filter.SortBy, nameof(Building.Price)),
            filter.PageIndex,
            filter.PageSize,
            WithCardGraph,
            cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Building>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Buildings
            .AsNoTracking()
            .Select(x => new { x.Id, x.BuildingType, x.ListingType, x.Area })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Building>();

        return await PagedListingQuery.ToStripAsync(
            _context.Buildings
                .AsNoTracking()
                .Where(x => x.Id != id && x.BuildingType == listing.BuildingType)
                .OrderByDescending(x => x.ListingType == listing.ListingType)
                .ThenBy(x => x.Area > listing.Area ? x.Area - listing.Area : listing.Area - x.Area)
                .ThenByDescending(x => x.CreatedAt),
            count,
            WithCardGraph,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Building>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Buildings
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.Center })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Building>();

        return await PagedListingQuery.ToStripAsync(
            _context.Buildings
                .AsNoTracking()
                .Where(x => x.Id != id && (x.UserId == listing.UserId || x.Center == listing.Center))
                .OrderByDescending(x => x.UserId == listing.UserId)
                .ThenByDescending(x => x.CreatedAt),
            count,
            WithCardGraph,
            cancellationToken);
    }

    public async Task<IReadOnlyList<Building>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await WithCardGraph(_context.Buildings
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .Take(count))
            .ToListAsync(cancellationToken);

    public async Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        BuildingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Buildings.AsNoTracking(), filter);

        var statistics = await query
            .GroupBy(_ => 1)
            .Select(group => new RealEstatePriceStatisticsDto
            {
                Count = group.Count(),
                MinPrice = group.Min(x => x.Price),
                MaxPrice = group.Max(x => x.Price),
                AveragePrice = group.Average(x => x.Price)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return statistics ?? new RealEstatePriceStatisticsDto();
    }

    public async Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default) =>
        await _context.Buildings
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

    public Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync<TLookup>(
        CancellationToken cancellationToken = default)
        where TLookup : class, IRealEstateLookup =>
        RealEstateLookupReader.ReadAsync<TLookup>(_context, cancellationToken);

    public async Task AddAsync(Building entity) => await _context.Buildings.AddAsync(entity);

    public void Update(Building entity) => _context.Buildings.Update(entity);

    public void RemoveImage(BuildingImage image) => _context.BuildingImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<BuildingImage> images) =>
        await _context.BuildingImages.AddRangeAsync(images);

    public void RemoveFeatures(IEnumerable<BuildingFeatureSelection> features) =>
        _context.BuildingFeatureSelections.RemoveRange(features);

    public async Task AddFeaturesAsync(IEnumerable<BuildingFeatureSelection> features) =>
        await _context.BuildingFeatureSelections.AddRangeAsync(features);

    public void RemoveRentInclusions(IEnumerable<BuildingRentInclusionSelection> inclusions) =>
        _context.BuildingRentInclusionSelections.RemoveRange(inclusions);

    public async Task AddRentInclusionsAsync(IEnumerable<BuildingRentInclusionSelection> inclusions) =>
        await _context.BuildingRentInclusionSelections.AddRangeAsync(inclusions);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<Building> WithCardGraph(IQueryable<Building> query) =>
        query
            .Include(x => x.Images)
            .Include(x => x.Features)
            .AsSplitQuery();

    private static IQueryable<Building> Filter(IQueryable<Building> query, BuildingFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                EF.Functions.Like(x.AdvertiserName, $"%{search}%") ||
                (x.District != null && EF.Functions.Like(x.District, $"%{search}%")) ||
                EF.Functions.Like(x.Address, $"%{search}%") ||
                (x.OtherProject != null && EF.Functions.Like(x.OtherProject, $"%{search}%")));
        }

        if (!string.IsNullOrWhiteSpace(filter.AdvertiserName))
        {
            var advertiserName = filter.AdvertiserName.Trim();
            query = query.Where(x => EF.Functions.Like(x.AdvertiserName, $"%{advertiserName}%"));
        }

        if (filter.BuildingType is { } buildingType)
            query = query.Where(x => x.BuildingType == buildingType);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        if (filter.AreaFrom is { } areaFrom)
            query = query.Where(x => x.Area >= areaFrom);

        if (filter.AreaTo is { } areaTo)
            query = query.Where(x => x.Area <= areaTo);

        if (filter.FloorsCount is { } floorsCount)
            query = query.Where(x => x.FloorsCount == floorsCount);

        if (filter.RoomsCount is { } roomsCount)
            query = query.Where(x => x.RoomsCount == roomsCount);

        if (filter.BathroomsCount is { } bathroomsCount)
            query = query.Where(x => x.BathroomsCount == bathroomsCount);

        if (filter.FinishingType is { } finishingType)
            query = query.Where(x => x.FinishingType == finishingType);

        if (filter.FurnishedStatus is { } furnishedStatus)
            query = query.Where(x => x.FurnishedStatus == furnishedStatus);

        if (filter.PropertyAge is { } propertyAge)
            query = query.Where(x => x.PropertyAge == propertyAge);

        query = ApplyFeatureFilter(query, filter.HasElevator, ApartmentFeature.Elevator);
        query = ApplyFeatureFilter(query, filter.HasGarage, ApartmentFeature.Garage);
        query = ApplyFeatureFilter(query, filter.HasSwimmingPool, ApartmentFeature.SwimmingPool);
        query = ApplyFeatureFilter(query, filter.HasGarden, ApartmentFeature.Garden);

        if (filter.LegalStatus is { } legalStatus)
            query = query.Where(x => x.LegalStatus == legalStatus);

        if (filter.OwnershipDocument is { } ownershipDocument)
            query = query.Where(x => x.OwnershipDocument == ownershipDocument);

        return RealEstateQueries.ApplyCommonFilters(query, filter);
    }

    private static IQueryable<Building> ApplyFeatureFilter(
        IQueryable<Building> query, bool? wanted, ApartmentFeature feature) =>
        wanted switch
        {
            true => query.Where(x => x.Features.Any(selection => selection.Feature == feature)),
            false => query.Where(x => !x.Features.Any(selection => selection.Feature == feature)),
            _ => query
        };
}
