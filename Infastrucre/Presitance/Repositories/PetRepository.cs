using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Animals;

namespace Persistence.Repositories;

public class PetRepository : IPetRepository
{
    private readonly AppDbContext _context;

    public PetRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Pet?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Pet> query = _context.Pets.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Pet?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Pets
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Pet> Items, int TotalCount)> GetPagedAsync(
        PetFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Pets.AsNoTracking().AsQueryable();

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
            return (Array.Empty<Pet>(), 0);

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

    public Task<List<PetBreedLookup>> GetBreedsAsync(CancellationToken cancellationToken = default) =>
        _context.PetBreeds.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PetPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default) =>
        _context.PetPurposes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PetAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default) =>
        _context.PetAges.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PetGenderLookup>> GetGendersAsync(CancellationToken cancellationToken = default) =>
        _context.PetGenders.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PetHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default) =>
        _context.PetHealthStatuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PetTrainingLevelLookup>> GetTrainingLevelsAsync(CancellationToken cancellationToken = default) =>
        _context.PetTrainingLevels.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PetVaccinationLookup>> GetVaccinationsAsync(CancellationToken cancellationToken = default) =>
        _context.PetVaccinations.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Pet entity) => await _context.Pets.AddAsync(entity);

    public void Update(Pet entity) => _context.Pets.Update(entity);

    public void RemoveImage(PetImage image) => _context.PetImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<PetImage> images) =>
        await _context.PetImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
