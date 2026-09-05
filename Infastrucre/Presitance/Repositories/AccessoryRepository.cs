using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.OnlineShopping;
using Shared.Enums;

namespace Persistence.Repositories;

public class AccessoryRepository : IAccessoryRepository
{
    private readonly AppDbContext _context;

    public AccessoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Accessory?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Accessory> query = _context.Accessories
            .Include(x => x.Images)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Accessory?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Accessories
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Accessory> Items, int TotalCount)> GetPagedAsync(
        AccessoryFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Accessories.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.StoreName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                (x.OtherAccessoryType != null && EF.Functions.Like(x.OtherAccessoryType, $"%{search}%")));
        }

        if (!string.IsNullOrWhiteSpace(filter.StoreName))
        {
            var storeName = filter.StoreName.Trim();
            query = query.Where(x => EF.Functions.Like(x.StoreName, $"%{storeName}%"));
        }

        if (filter.AccessoryType is { } accessoryType)
            query = query.Where(x => x.AccessoryType == accessoryType);

        if (filter.Category is { } category)
            query = query.Where(x => x.Category == category);

        if (filter.Material is { } material)
            query = query.Where(x => x.Material == material);

        if (filter.Color is { } color)
            query = query.Where(x => x.Colors.Any(selection => selection.Color == color));

        if (filter.ShippingAvailable is { } shippingAvailable)
            query = query.Where(x => x.ShippingAvailable == shippingAvailable);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<Accessory>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            OnlineShoppingSorting.Apply(query, filter.SortBy, x => x.Price, x => x.CreatedAt, x => x.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .Include(x => x.Colors)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<AccessoryTypeLookup>> GetAccessoryTypesAsync(CancellationToken cancellationToken = default) =>
        _context.AccessoryTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<AccessoryCategoryLookup>> GetCategoriesAsync(CancellationToken cancellationToken = default) =>
        _context.AccessoryCategories.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<AccessoryMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default) =>
        _context.AccessoryMaterials.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<AccessoryColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.AccessoryColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Accessory entity) => await _context.Accessories.AddAsync(entity);

    public void Update(Accessory entity) => _context.Accessories.Update(entity);

    public void RemoveImage(AccessoryImage image) => _context.AccessoryImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<AccessoryImage> images) =>
        await _context.AccessoryImages.AddRangeAsync(images);

    public void RemoveColors(IEnumerable<AccessoryColorSelection> colors) =>
        _context.AccessoryColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<AccessoryColorSelection> colors) =>
        await _context.AccessoryColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
