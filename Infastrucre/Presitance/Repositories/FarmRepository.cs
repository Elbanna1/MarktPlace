using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Farms;

namespace Persistence.Repositories;

public class FarmRepository : IFarmRepository
{
    private readonly AppDbContext _context;

    public FarmRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Farm?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Farm> query = _context.Farms.Include(f => f.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public Task<Farm?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Farms
            .Include(f => f.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Farm> Items, int TotalCount)> GetPagedAsync(
        FarmFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Farms.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.FarmName))
        {
            var name = filter.FarmName.Trim();
            query = query.Where(f => EF.Functions.Like(f.FarmName, $"%{name}%"));
        }

        if (filter.FarmType is { } farmType)
            query = query.Where(f => f.FarmType == farmType);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(f =>
                EF.Functions.Like(f.FarmName, $"%{term}%") ||
                EF.Functions.Like(f.Title, $"%{term}%") ||
                EF.Functions.Like(f.Description, $"%{term}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Farm>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(f => f.CreatedAt)
                .ThenByDescending(f => f.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(f => f.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<FarmTypeLookup>> GetFarmTypesAsync(CancellationToken cancellationToken = default) =>
        _context.FarmTypes.AsNoTracking().OrderBy(f => f.Id).ToListAsync(cancellationToken);

    public Task<List<FarmingMethodLookup>> GetFarmingMethodsAsync(CancellationToken cancellationToken = default) =>
        _context.FarmingMethods.AsNoTracking().OrderBy(f => f.Id).ToListAsync(cancellationToken);

    public Task<List<AvailabilitySeasonLookup>> GetAvailabilitySeasonsAsync(
        CancellationToken cancellationToken = default) =>
        _context.AvailabilitySeasons.AsNoTracking().OrderBy(a => a.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Farm farm) => await _context.Farms.AddAsync(farm);

    public void Update(Farm farm) => _context.Farms.Update(farm);

    public void RemoveImage(FarmImage image) => _context.FarmImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<FarmImage> images) =>
        await _context.FarmImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
