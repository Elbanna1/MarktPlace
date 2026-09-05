using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Craftsmen;

namespace Persistence.Repositories;

public class CraftsmanRepository : ICraftsmanRepository
{
    private readonly AppDbContext _context;

    public CraftsmanRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Craftsman?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Craftsman> query = _context.Craftsmen.Include(c => c.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Craftsman?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Craftsmen
            .Include(c => c.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Craftsman> Items, int TotalCount)> GetPagedAsync(
        CraftsmanFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Craftsmen.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(c =>
                EF.Functions.Like(c.Name, $"%{search}%") ||
                EF.Functions.Like(c.AdTitle, $"%{search}%") ||
                EF.Functions.Like(c.AdDescription, $"%{search}%"));
        }

        if (filter.Specialization is { } specialization)
            query = query.Where(c => c.Specialization == specialization);

        if (filter.ExperienceLevel is { } experience)
            query = query.Where(c => c.ExperienceLevel == experience);

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var city = filter.City.Trim();
            query = query.Where(c => c.Center == city);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Craftsman>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(c => c.CreatedAt)
                .ThenByDescending(c => c.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(c => c.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Craftsman craftsman) =>
        await _context.Craftsmen.AddAsync(craftsman);

    public void Update(Craftsman craftsman) =>
        _context.Craftsmen.Update(craftsman);

    public void RemoveImage(CraftsmanImage image) =>
        _context.CraftsmanImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<CraftsmanImage> images) =>
        await _context.CraftsmanImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
