using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.OnlineShopping;

namespace Persistence.Repositories;

public class HomeKitchenRepository : IHomeKitchenRepository
{
    private readonly AppDbContext _context;

    public HomeKitchenRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<HomeKitchen?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<HomeKitchen> query = _context.HomeKitchens
            .Include(x => x.Images)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<HomeKitchen?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.HomeKitchens
            .Include(x => x.Images)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<HomeKitchen> Items, int TotalCount)> GetPagedAsync(
        HomeKitchenFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.HomeKitchens.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.StoreName, $"%{search}%") ||
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

        if (filter.Material is { } material)
            query = query.Where(x => x.Material == material);

        if (filter.Color is { } color)
            query = query.Where(x => x.Colors.Any(selection => selection.Color == color));

        if (filter.DeliveryAvailable is { } deliveryAvailable)
            query = query.Where(x => x.DeliveryAvailable == deliveryAvailable);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<HomeKitchen>(), 0);

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

    public Task<List<HomeKitchenSectionLookup>> GetSectionsAsync(CancellationToken cancellationToken = default) =>
        _context.HomeKitchenSections.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HomeKitchenMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default) =>
        _context.HomeKitchenMaterials.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HomeKitchenColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.HomeKitchenColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(HomeKitchen entity) => await _context.HomeKitchens.AddAsync(entity);

    public void Update(HomeKitchen entity) => _context.HomeKitchens.Update(entity);

    public void RemoveImage(HomeKitchenImage image) => _context.HomeKitchenImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<HomeKitchenImage> images) =>
        await _context.HomeKitchenImages.AddRangeAsync(images);

    public void RemoveColors(IEnumerable<HomeKitchenColorSelection> colors) =>
        _context.HomeKitchenColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<HomeKitchenColorSelection> colors) =>
        await _context.HomeKitchenColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
