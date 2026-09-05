using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Suppliers;

namespace Persistence.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Supplier?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Supplier> query = _context.Suppliers.Include(s => s.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public Task<Supplier?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Suppliers
            .Include(s => s.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Supplier> Items, int TotalCount)> GetPagedAsync(
        SupplierFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Suppliers.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(s =>
                EF.Functions.Like(s.SupplierName, $"%{search}%") ||
                EF.Functions.Like(s.SuppliedProduct, $"%{search}%") ||
                EF.Functions.Like(s.SupplyDetails, $"%{search}%") ||
                EF.Functions.Like(s.Title, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.SupplierName))
        {
            var supplierName = filter.SupplierName.Trim();
            query = query.Where(s => EF.Functions.Like(s.SupplierName, $"%{supplierName}%"));
        }

        if (filter.SupplierType is { } supplierType)
            query = query.Where(s => s.SupplierType == supplierType);

        if (!string.IsNullOrWhiteSpace(filter.SuppliedProduct))
        {
            var product = filter.SuppliedProduct.Trim();
            query = query.Where(s => EF.Functions.Like(s.SuppliedProduct, $"%{product}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Supplier>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(s => s.CreatedAt)
                .ThenByDescending(s => s.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(s => s.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<SupplierSpecializationLookup>> GetSpecializationsAsync(
        CancellationToken cancellationToken = default) =>
        _context.SupplierSpecializations
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Supplier supplier) =>
        await _context.Suppliers.AddAsync(supplier);

    public void Update(Supplier supplier) =>
        _context.Suppliers.Update(supplier);

    public void RemoveImage(SupplierImage image) =>
        _context.SupplierImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<SupplierImage> images) =>
        await _context.SupplierImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
