using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;

namespace Persistence.Repositories;

public class BannerRepository : IBannerRepository
{
    private readonly AppDbContext _context;

    public BannerRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Banner?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Banner> query = _context.Banners;

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Banner>> GetActiveAsync(
        DateTime utcNow, CancellationToken cancellationToken = default) =>
        await Ordered(
                _context.Banners
                    .AsNoTracking()
                    .Where(b => b.IsActive
                                && (b.StartDate == null || b.StartDate <= utcNow)
                                && (b.EndDate == null || b.EndDate >= utcNow)))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Banner>> GetAllAsync(
        bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Banners.AsNoTracking().AsQueryable();

        if (isActive is { } wanted)
            query = query.Where(b => b.IsActive == wanted);

        return await Ordered(query).ToListAsync(cancellationToken);
    }

    private static IQueryable<Banner> Ordered(IQueryable<Banner> query) =>
        query
            .OrderBy(b => b.DisplayOrder)
            .ThenBy(b => b.CreatedAt)
            .ThenBy(b => b.Id);

    public void Add(Banner banner) => _context.Banners.Add(banner);

    public void Update(Banner banner) => _context.Banners.Update(banner);

    public void Remove(Banner banner) => _context.Banners.Remove(banner);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}
