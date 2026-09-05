using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Factories;

namespace Persistence.Repositories;

public class FactoryRepository : IFactoryRepository
{
    private readonly AppDbContext _context;

    public FactoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Factory?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Factory> query = _context.Factories.Include(f => f.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public Task<Factory?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Factories
            .Include(f => f.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Factory> Items, int TotalCount)> GetPagedAsync(
        FactoryFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Factories.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.FactoryName))
        {
            var name = filter.FactoryName.Trim();
            query = query.Where(f => EF.Functions.Like(f.FactoryName, $"%{name}%"));
        }

        if (filter.ProductionSpecialty is { } specialty)
            query = query.Where(f => f.ProductionSpecialty == specialty);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(f =>
                EF.Functions.Like(f.FactoryName, $"%{term}%") ||
                EF.Functions.Like(f.Title, $"%{term}%") ||
                EF.Functions.Like(f.Description, $"%{term}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Factory>(), 0);

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

    public Task<List<ProductionSpecialtyLookup>> GetProductionSpecialtiesAsync(
        CancellationToken cancellationToken = default) =>
        _context.ProductionSpecialties.AsNoTracking()
            .OrderBy(p => p.Id == BusinessCatalog.ProductionSpecialtyLastId ? 1 : 0)
            .ThenBy(p => p.Id)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Factory factory) =>
        await _context.Factories.AddAsync(factory);

    public void Update(Factory factory) =>
        _context.Factories.Update(factory);

    public void RemoveImage(FactoryImage image) =>
        _context.FactoryImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<FactoryImage> images) =>
        await _context.FactoryImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
