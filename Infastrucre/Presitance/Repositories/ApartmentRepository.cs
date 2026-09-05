using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.RealEstate;
using Shared.Enums;

namespace Persistence.Repositories;

public class ApartmentRepository : IApartmentRepository
{
    private readonly AppDbContext _context;

    public ApartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Apartment?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Apartment> query = _context.Apartments
            .Include(x => x.Images)
            .Include(x => x.Features)
            .Include(x => x.RentInclusions);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Apartment?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Apartments
            .Include(x => x.Images)
            .Include(x => x.Features)
            .Include(x => x.RentInclusions)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Apartment> Items, int TotalCount)> GetPagedAsync(
        ApartmentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Apartments.AsNoTracking(), filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Apartment>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            RealEstateQueries.ApplyOrdering(query, filter.SortBy, nameof(Apartment.Price)),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .Include(x => x.Features)
                .Include(x => x.RentInclusions)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Apartment>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Apartments
            .AsNoTracking()
            .Select(x => new { x.Id, x.ApartmentType, x.Price })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Apartment>();

        return await PagedListingQuery.ToStripAsync(
            _context.Apartments
                .AsNoTracking()
                .Where(x => x.Id != id && x.ApartmentType == listing.ApartmentType)
                .OrderBy(x => x.Price > listing.Price ? x.Price - listing.Price : listing.Price - x.Price)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Features)
                .Include(x => x.RentInclusions)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<Apartment>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Apartments
            .AsNoTracking()
            .Select(x => new { x.Id, x.UserId, x.Center })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (listing is null)
            return Array.Empty<Apartment>();

        return await PagedListingQuery.ToStripAsync(
            _context.Apartments
                .AsNoTracking()
                .Where(x => x.Id != id && (x.UserId == listing.UserId || x.Center == listing.Center))
                .OrderByDescending(x => x.UserId == listing.UserId)
                .ThenByDescending(x => x.CreatedAt),
            count,
            query => query
                .Include(x => x.Images)
                .Include(x => x.Features)
                .Include(x => x.RentInclusions)
                .AsSplitQuery(),
            cancellationToken);
    }

    public async Task<IReadOnlyList<Apartment>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default) =>
        await _context.Apartments
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Take(count)
            .Include(x => x.Images)
            .Include(x => x.Features)
            .Include(x => x.RentInclusions)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    public async Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        ApartmentFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(_context.Apartments.AsNoTracking(), filter);

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
        await _context.Apartments
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
        _context.Apartments
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);

    public Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync<TLookup>(
        CancellationToken cancellationToken = default)
        where TLookup : class, IRealEstateLookup =>
        RealEstateLookupReader.ReadAsync<TLookup>(_context, cancellationToken);

    public async Task AddAsync(Apartment entity) => await _context.Apartments.AddAsync(entity);

    public void Update(Apartment entity) => _context.Apartments.Update(entity);

    public void RemoveImage(ApartmentImage image) => _context.ApartmentImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<ApartmentImage> images) =>
        await _context.ApartmentImages.AddRangeAsync(images);

    public void RemoveFeatures(IEnumerable<ApartmentFeatureSelection> features) =>
        _context.ApartmentFeatureSelections.RemoveRange(features);

    public async Task AddFeaturesAsync(IEnumerable<ApartmentFeatureSelection> features) =>
        await _context.ApartmentFeatureSelections.AddRangeAsync(features);

    public void RemoveRentInclusions(IEnumerable<ApartmentRentInclusionSelection> inclusions) =>
        _context.ApartmentRentInclusionSelections.RemoveRange(inclusions);

    public async Task AddRentInclusionsAsync(IEnumerable<ApartmentRentInclusionSelection> inclusions) =>
        await _context.ApartmentRentInclusionSelections.AddRangeAsync(inclusions);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private static IQueryable<Apartment> Filter(IQueryable<Apartment> query, ApartmentFilterParams filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                (x.District != null && EF.Functions.Like(x.District, $"%{search}%")) ||
                (x.Address != null && EF.Functions.Like(x.Address, $"%{search}%")) ||
                (x.OtherApartmentType != null && EF.Functions.Like(x.OtherApartmentType, $"%{search}%")) ||
                (x.OtherProject != null && EF.Functions.Like(x.OtherProject, $"%{search}%")));
        }

        if (filter.ApartmentType is { } apartmentType)
            query = query.Where(x => x.ApartmentType == apartmentType);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        if (filter.AreaFrom is { } areaFrom)
            query = query.Where(x => x.Area >= areaFrom);

        if (filter.AreaTo is { } areaTo)
            query = query.Where(x => x.Area <= areaTo);

        if (filter.RoomsCount is { } roomsCount)
            query = query.Where(x => x.RoomsCount == roomsCount);

        if (filter.BathroomsCount is { } bathroomsCount)
            query = query.Where(x => x.BathroomsCount == bathroomsCount);

        if (filter.FloorType is { } floorType)
            query = query.Where(x => x.FloorType == floorType);

        if (filter.FinishingType is { } finishingType)
            query = query.Where(x => x.FinishingType == finishingType);

        if (filter.FurnishedStatus is { } furnishedStatus)
            query = query.Where(x => x.FurnishedStatus == furnishedStatus);

        query = ApplyFeatureFilter(query, filter.HasElevator, ApartmentFeature.Elevator);
        query = ApplyFeatureFilter(query, filter.HasGarage, ApartmentFeature.Garage);
        query = ApplyFeatureFilter(query, filter.HasNaturalGas, ApartmentFeature.NaturalGas);
        query = ApplyFeatureFilter(query, filter.HasAirConditioning, ApartmentFeature.AirConditioning);
        query = ApplyFeatureFilter(query, filter.HasBalcony, ApartmentFeature.Balcony);

        if (filter.OwnershipType is { } ownershipType)
            query = query.Where(x => x.OwnershipType == ownershipType);

        if (filter.LegalStatus is { } legalStatus)
            query = query.Where(x => x.LegalStatus == legalStatus);

        if (filter.OwnershipDocument is { } ownershipDocument)
            query = query.Where(x => x.OwnershipDocument == ownershipDocument);

        return RealEstateQueries.ApplyCommonFilters(query, filter);
    }

    private static IQueryable<Apartment> ApplyFeatureFilter(
        IQueryable<Apartment> query, bool? wanted, ApartmentFeature feature) =>
        wanted switch
        {
            true => query.Where(x => x.Features.Any(selection => selection.Feature == feature)),
            false => query.Where(x => !x.Features.Any(selection => selection.Feature == feature)),
            _ => query
        };
}
