using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Animals;

namespace Persistence.Repositories;

public class LivestockRepository : ILivestockRepository
{
    private readonly AppDbContext _context;

    public LivestockRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Livestock?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Livestock> query = _context.Livestock.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Livestock?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Livestock
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Livestock> Items, int TotalCount)> GetPagedAsync(
        LivestockFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Livestock.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.SellerName))
        {
            var sellerName = filter.SellerName.Trim();
            query = query.Where(x => EF.Functions.Like(x.SellerName, $"%{sellerName}%"));
        }

        if (filter.Breed is { } breed)
            query = query.Where(x => x.Breed == breed);

        if (filter.Purpose is { } purpose)
            query = query.Where(x => x.Purpose == purpose);

        if (filter.Age is { } age)
            query = query.Where(x => x.Age == age);

        if (filter.Gender is { } gender)
            query = query.Where(x => x.Gender == gender);

        if (filter.HealthStatus is { } healthStatus)
            query = query.Where(x => x.HealthStatus == healthStatus);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Livestock>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<LivestockBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default) =>
        _context.LivestockBreeds.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<LivestockPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default) =>
        _context.LivestockPurposes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<LivestockAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default) =>
        _context.LivestockAges.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<LivestockGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default) =>
        _context.LivestockGenders.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<LivestockHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default) =>
        _context.LivestockHealthStatuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<LivestockVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default) =>
        _context.LivestockVaccinations.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<LivestockProductionLookup>> GetProductionsAsync(CancellationToken cancellationToken = default) =>
        _context.LivestockProductions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Livestock entity) => await _context.Livestock.AddAsync(entity);

    public void Update(Livestock entity) => _context.Livestock.Update(entity);

    public void RemoveImage(LivestockImage image) => _context.LivestockImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<LivestockImage> images) =>
        await _context.LivestockImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
