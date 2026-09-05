using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.FruitVegetableMerchants;

namespace Persistence.Repositories;

public class FruitVegetableMerchantRepository : IFruitVegetableMerchantRepository
{
    private readonly AppDbContext _context;

    public FruitVegetableMerchantRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<FruitVegetableMerchant?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default,
        bool includeUnmoderated = false)
    {
        IQueryable<FruitVegetableMerchant> query =
            _context.FruitVegetableMerchants.Include(m => m.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public Task<FruitVegetableMerchant?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.FruitVegetableMerchants
            .Include(m => m.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<FruitVegetableMerchant> Items, int TotalCount)> GetPagedAsync(
        FruitVegetableMerchantFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.FruitVegetableMerchants.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(m =>
                EF.Functions.Like(m.MerchantName, $"%{search}%") ||
                EF.Functions.Like(m.StallName, $"%{search}%") ||
                EF.Functions.Like(m.ProductName, $"%{search}%") ||
                EF.Functions.Like(m.ProductDetails, $"%{search}%") ||
                EF.Functions.Like(m.Title, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.MerchantName))
        {
            var merchantName = filter.MerchantName.Trim();
            query = query.Where(m => EF.Functions.Like(m.MerchantName, $"%{merchantName}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.StallName))
        {
            var stallName = filter.StallName.Trim();
            query = query.Where(m => EF.Functions.Like(m.StallName, $"%{stallName}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.ProductName))
        {
            var productName = filter.ProductName.Trim();
            query = query.Where(m => EF.Functions.Like(m.ProductName, $"%{productName}%"));
        }

        if (filter.SaleType is { } saleType)
            query = query.Where(m => m.SaleType == saleType);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<FruitVegetableMerchant>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(m => m.CreatedAt)
                .ThenByDescending(m => m.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(m => m.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<MerchantSaleTypeLookup>> GetSaleTypesAsync(
        CancellationToken cancellationToken = default) =>
        _context.MerchantSaleTypes
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(FruitVegetableMerchant merchant) =>
        await _context.FruitVegetableMerchants.AddAsync(merchant);

    public void Update(FruitVegetableMerchant merchant) =>
        _context.FruitVegetableMerchants.Update(merchant);

    public void RemoveImage(FruitVegetableMerchantImage image) =>
        _context.FruitVegetableMerchantImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<FruitVegetableMerchantImage> images) =>
        await _context.FruitVegetableMerchantImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
