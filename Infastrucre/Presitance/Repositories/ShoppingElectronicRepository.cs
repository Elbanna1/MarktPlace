using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.OnlineShopping;

namespace Persistence.Repositories;

public class ShoppingElectronicRepository : IShoppingElectronicRepository
{
    private readonly AppDbContext _context;

    public ShoppingElectronicRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<ShoppingElectronic?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<ShoppingElectronic> query = _context.ShoppingElectronics.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<ShoppingElectronic?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.ShoppingElectronics
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<ShoppingElectronic> Items, int TotalCount)> GetPagedAsync(
        ShoppingElectronicFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.ShoppingElectronics.AsNoTracking().AsQueryable();

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

        if (filter.CompatibleWith is { } compatibleWith)
            query = query.Where(x => x.CompatibleWith == compatibleWith);

        if (filter.ProductCondition is { } productCondition)
            query = query.Where(x => x.ProductCondition == productCondition);

        if (filter.Warranty is { } warranty)
            query = query.Where(x => x.Warranty == warranty);

        if (filter.ShippingAvailable is { } shippingAvailable)
            query = query.Where(x => x.ShippingAvailable == shippingAvailable);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<ShoppingElectronic>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            OnlineShoppingSorting.Apply(query, filter.SortBy, x => x.Price, x => x.CreatedAt, x => x.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<ShoppingElectronicSectionLookup>> GetSectionsAsync(
        CancellationToken cancellationToken = default) =>
        _context.ShoppingElectronicSections.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<ShoppingElectronicCompatibilityLookup>> GetCompatibilitiesAsync(
        CancellationToken cancellationToken = default) =>
        _context.ShoppingElectronicCompatibilities.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<ShoppingElectronicConditionLookup>> GetConditionsAsync(
        CancellationToken cancellationToken = default) =>
        _context.ShoppingElectronicConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<ShoppingElectronicWarrantyLookup>> GetWarrantiesAsync(
        CancellationToken cancellationToken = default) =>
        _context.ShoppingElectronicWarranties.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(ShoppingElectronic entity) => await _context.ShoppingElectronics.AddAsync(entity);

    public void Update(ShoppingElectronic entity) => _context.ShoppingElectronics.Update(entity);

    public void RemoveImage(ShoppingElectronicImage image) => _context.ShoppingElectronicImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<ShoppingElectronicImage> images) =>
        await _context.ShoppingElectronicImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
