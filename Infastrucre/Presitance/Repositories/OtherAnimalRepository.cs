using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Animals;

namespace Persistence.Repositories;

public class OtherAnimalRepository : IOtherAnimalRepository
{
    private readonly AppDbContext _context;

    public OtherAnimalRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<OtherAnimal?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<OtherAnimal> query = _context.OtherAnimals.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<OtherAnimal?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.OtherAnimals
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<OtherAnimal> Items, int TotalCount)> GetPagedAsync(
        OtherAnimalFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.OtherAnimals.AsNoTracking().AsQueryable();

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
            return (Array.Empty<OtherAnimal>(), 0);

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

    public Task<List<OtherAnimalTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default) =>
        _context.OtherAnimalTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<OtherAnimalPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default) =>
        _context.OtherAnimalPurposes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<OtherAnimalAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default) =>
        _context.OtherAnimalAges.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<OtherAnimalGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default) =>
        _context.OtherAnimalGenders.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<OtherAnimalHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default) =>
        _context.OtherAnimalHealthStatuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<OtherAnimalVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default) =>
        _context.OtherAnimalVaccinations.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(OtherAnimal entity) => await _context.OtherAnimals.AddAsync(entity);

    public void Update(OtherAnimal entity) => _context.OtherAnimals.Update(entity);

    public void RemoveImage(OtherAnimalImage image) => _context.OtherAnimalImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<OtherAnimalImage> images) =>
        await _context.OtherAnimalImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
