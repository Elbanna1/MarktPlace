using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Animals;

namespace Persistence.Repositories;

public class CamelRepository : ICamelRepository
{
    private readonly AppDbContext _context;

    public CamelRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Camel?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Camel> query = _context.Camels.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Camel?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Camels
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Camel> Items, int TotalCount)> GetPagedAsync(
        CamelFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Camels.AsNoTracking().AsQueryable();

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
            return (Array.Empty<Camel>(), 0);

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

    public Task<List<CamelBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default) =>
        _context.CamelBreeds.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<CamelPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default) =>
        _context.CamelPurposes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<CamelAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default) =>
        _context.CamelAges.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<CamelGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default) =>
        _context.CamelGenders.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<CamelHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default) =>
        _context.CamelHealthStatuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<CamelVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default) =>
        _context.CamelVaccinations.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Camel entity) => await _context.Camels.AddAsync(entity);

    public void Update(Camel entity) => _context.Camels.Update(entity);

    public void RemoveImage(CamelImage image) => _context.CamelImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<CamelImage> images) =>
        await _context.CamelImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
