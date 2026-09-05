using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Animals;

namespace Persistence.Repositories;

public class HorseRepository : IHorseRepository
{
    private readonly AppDbContext _context;

    public HorseRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Horse?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Horse> query = _context.Horses.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Horse?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Horses
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Horse> Items, int TotalCount)> GetPagedAsync(
        HorseFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Horses.AsNoTracking().AsQueryable();

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
            return (Array.Empty<Horse>(), 0);

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

    public Task<List<HorseBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default) =>
        _context.HorseBreeds.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HorsePurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default) =>
        _context.HorsePurposes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HorseAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default) =>
        _context.HorseAges.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HorseGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default) =>
        _context.HorseGenders.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HorseHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default) =>
        _context.HorseHealthStatuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HorseTrainingLevelLookup>> GetTrainingLevelsAsync(CancellationToken cancellationToken = default) =>
        _context.HorseTrainingLevels.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HorseVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default) =>
        _context.HorseVaccinations.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Horse entity) => await _context.Horses.AddAsync(entity);

    public void Update(Horse entity) => _context.Horses.Update(entity);

    public void RemoveImage(HorseImage image) => _context.HorseImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<HorseImage> images) =>
        await _context.HorseImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
