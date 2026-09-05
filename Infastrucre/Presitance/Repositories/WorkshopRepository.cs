using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Workshops;

namespace Persistence.Repositories;

public class WorkshopRepository : IWorkshopRepository
{
    private readonly AppDbContext _context;

    public WorkshopRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Workshop?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Workshop> query = _context.Workshops.Include(w => w.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public Task<Workshop?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Workshops
            .Include(w => w.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Workshop> Items, int TotalCount)> GetPagedAsync(
        WorkshopFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Workshops.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(w =>
                EF.Functions.Like(w.Name, $"%{search}%") ||
                EF.Functions.Like(w.AdTitle, $"%{search}%") ||
                EF.Functions.Like(w.AdDescription, $"%{search}%"));
        }

        if (filter.WorkshopType is { } type)
            query = query.Where(w => w.WorkshopType == type);

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var city = filter.City.Trim();
            query = query.Where(w => w.Center == city);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Workshop>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(w => w.CreatedAt)
                .ThenByDescending(w => w.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(w => w.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Workshop workshop) =>
        await _context.Workshops.AddAsync(workshop);

    public void Update(Workshop workshop) =>
        _context.Workshops.Update(workshop);

    public void RemoveImage(WorkshopImage image) =>
        _context.WorkshopImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<WorkshopImage> images) =>
        await _context.WorkshopImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
