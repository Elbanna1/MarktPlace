using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Animals;

namespace Persistence.Repositories;

public class SheepGoatRepository : ISheepGoatRepository
{
    private readonly AppDbContext _context;

    public SheepGoatRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<SheepGoat?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<SheepGoat> query = _context.SheepGoats.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<SheepGoat?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.SheepGoats
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<SheepGoat> Items, int TotalCount)> GetPagedAsync(
        SheepGoatFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.SheepGoats.AsNoTracking().AsQueryable();

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
            return (Array.Empty<SheepGoat>(), 0);

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

    public Task<List<SheepGoatBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default) =>
        _context.SheepGoatBreeds.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<SheepGoatPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default) =>
        _context.SheepGoatPurposes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<SheepGoatAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default) =>
        _context.SheepGoatAges.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<SheepGoatGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default) =>
        _context.SheepGoatGenders.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<SheepGoatHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default) =>
        _context.SheepGoatHealthStatuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<SheepGoatVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default) =>
        _context.SheepGoatVaccinations.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(SheepGoat entity) => await _context.SheepGoats.AddAsync(entity);

    public void Update(SheepGoat entity) => _context.SheepGoats.Update(entity);

    public void RemoveImage(SheepGoatImage image) => _context.SheepGoatImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<SheepGoatImage> images) =>
        await _context.SheepGoatImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
