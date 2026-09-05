using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Companies;

namespace Persistence.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Company?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Company> query = _context.Companies.Include(c => c.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Company?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Companies
            .Include(c => c.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Company> Items, int TotalCount)> GetPagedAsync(
        CompanyFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Companies.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.CompanyName))
        {
            var name = filter.CompanyName.Trim();
            query = query.Where(c => EF.Functions.Like(c.CompanyName, $"%{name}%"));
        }

        if (filter.CompanyField is { } field)
            query = query.Where(c => c.CompanyField == field);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(c =>
                EF.Functions.Like(c.CompanyName, $"%{term}%") ||
                EF.Functions.Like(c.Title, $"%{term}%") ||
                EF.Functions.Like(c.Description, $"%{term}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Company>(), 0);

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

    public Task<List<CompanyFieldLookup>> GetCompanyFieldsAsync(CancellationToken cancellationToken = default) =>
        _context.CompanyFields.AsNoTracking().OrderBy(c => c.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Company company) => await _context.Companies.AddAsync(company);

    public void Update(Company company) => _context.Companies.Update(company);

    public void RemoveImage(CompanyImage image) => _context.CompanyImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<CompanyImage> images) =>
        await _context.CompanyImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
