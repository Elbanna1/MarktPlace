using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.OnlineShopping;

namespace Persistence.Repositories;

public class CosmeticRepository : ICosmeticRepository
{
    private readonly AppDbContext _context;

    public CosmeticRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Cosmetic?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Cosmetic> query = _context.Cosmetics.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Cosmetic?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Cosmetics
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Cosmetic> Items, int TotalCount)> GetPagedAsync(
        CosmeticFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Cosmetics.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.StoreName, $"%{search}%") ||
                EF.Functions.Like(x.Brand, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                (x.OtherSection != null && EF.Functions.Like(x.OtherSection, $"%{search}%")));
        }

        if (!string.IsNullOrWhiteSpace(filter.StoreName))
        {
            var storeName = filter.StoreName.Trim();
            query = query.Where(x => EF.Functions.Like(x.StoreName, $"%{storeName}%"));
        }

        if (filter.Section is { } section)
            query = query.Where(x => x.Section == section);

        if (!string.IsNullOrWhiteSpace(filter.Brand))
        {
            var brand = filter.Brand.Trim();
            query = query.Where(x => EF.Functions.Like(x.Brand, $"%{brand}%"));
        }

        if (filter.SuitableFor is { } suitableFor)
            query = query.Where(x => x.SuitableFor == suitableFor);

        if (filter.DiscountAvailable is { } discountAvailable)
            query = query.Where(x => x.DiscountAvailable == discountAvailable);

        if (filter.ShippingAvailable is { } shippingAvailable)
            query = query.Where(x => x.ShippingAvailable == shippingAvailable);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Cosmetic>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            OnlineShoppingSorting.Apply(query, filter.SortBy, x => x.Price, x => x.CreatedAt, x => x.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<CosmeticSectionLookup>> GetSectionsAsync(CancellationToken cancellationToken = default) =>
        _context.CosmeticSections.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<CosmeticSuitableForLookup>> GetSuitableForAsync(CancellationToken cancellationToken = default) =>
        _context.CosmeticSuitableFor.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Cosmetic entity) => await _context.Cosmetics.AddAsync(entity);

    public void Update(Cosmetic entity) => _context.Cosmetics.Update(entity);

    public void RemoveImage(CosmeticImage image) => _context.CosmeticImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<CosmeticImage> images) =>
        await _context.CosmeticImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
