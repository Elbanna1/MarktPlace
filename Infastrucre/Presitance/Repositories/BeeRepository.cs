using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Animals;

namespace Persistence.Repositories;

public class BeeRepository : IBeeRepository
{
    private readonly AppDbContext _context;

    public BeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Bee?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Bee> query = _context.Bees.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Bee?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Bees
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Bee> Items, int TotalCount)> GetPagedAsync(
        BeeFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Bees.AsNoTracking().AsQueryable();

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

        if (filter.AnimalType is { } animalType)
            query = query.Where(x => x.AnimalType == animalType);

        if (filter.Purpose is { } purpose)
            query = query.Where(x => x.Purpose == purpose);

        if (filter.HealthStatus is { } healthStatus)
            query = query.Where(x => x.HealthStatus == healthStatus);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Bee>(), 0);

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

    public Task<List<BeeTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default) =>
        _context.BeeTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<BeePurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default) =>
        _context.BeePurposes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<BeeHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default) =>
        _context.BeeHealthStatuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<BeeProductionLookup>> GetProductionsAsync(CancellationToken cancellationToken = default) =>
        _context.BeeProductions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Bee entity) => await _context.Bees.AddAsync(entity);

    public void Update(Bee entity) => _context.Bees.Update(entity);

    public void RemoveImage(BeeImage image) => _context.BeeImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<BeeImage> images) =>
        await _context.BeeImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
