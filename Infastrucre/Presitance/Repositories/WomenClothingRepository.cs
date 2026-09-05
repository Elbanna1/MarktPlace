using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Clothing;

namespace Persistence.Repositories;

public class WomenClothingRepository : IWomenClothingRepository
{
    private readonly AppDbContext _context;

    public WomenClothingRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<WomenClothing?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<WomenClothing> query = _context.WomenClothings
            .Include(x => x.Images)
            .Include(x => x.Sizes)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<WomenClothing?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.WomenClothings
            .Include(x => x.Images)
            .Include(x => x.Sizes)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<WomenClothing> Items, int TotalCount)> GetPagedAsync(
        WomenClothingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.WomenClothings.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.StoreName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                (x.OtherBrand != null && EF.Functions.Like(x.OtherBrand, $"%{search}%")));
        }

        if (!string.IsNullOrWhiteSpace(filter.StoreName))
        {
            var storeName = filter.StoreName.Trim();
            query = query.Where(x => EF.Functions.Like(x.StoreName, $"%{storeName}%"));
        }

        if (filter.ClothingType is { } clothingType)
            query = query.Where(x => x.ClothingType == clothingType);

        if (filter.Brand is { } brand)
            query = query.Where(x => x.Brand == brand);

        if (filter.Size is { } size)
            query = query.Where(x => x.Sizes.Any(selection => selection.Size == size));

        if (filter.Color is { } color)
            query = query.Where(x => x.Colors.Any(selection => selection.Color == color));

        if (filter.SellingMethod is { } sellingMethod)
            query = query.Where(x => x.SellingMethod == sellingMethod);

        if (!string.IsNullOrWhiteSpace(filter.Center))
        {
            var center = filter.Center.Trim();
            query = query.Where(x => x.Center == center);
        }

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<WomenClothing>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .Include(x => x.Sizes)
                .Include(x => x.Colors)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<WomenClothingTypeLookup>> GetClothingTypesAsync(CancellationToken cancellationToken = default) =>
        _context.WomenClothingTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<WomenClothingBrandLookup>> GetBrandsAsync(CancellationToken cancellationToken = default) =>
        _context.WomenClothingBrands.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<WomenClothingSizeLookup>> GetSizesAsync(CancellationToken cancellationToken = default) =>
        _context.WomenClothingSizes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<WomenClothingColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.WomenClothingColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<WomenClothingConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default) =>
        _context.WomenClothingConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<WomenClothingSellingMethodLookup>> GetSellingMethodsAsync(CancellationToken cancellationToken = default) =>
        _context.WomenClothingSellingMethods.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(WomenClothing entity) => await _context.WomenClothings.AddAsync(entity);

    public void Update(WomenClothing entity) => _context.WomenClothings.Update(entity);

    public void RemoveImage(WomenClothingImage image) => _context.WomenClothingImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<WomenClothingImage> images) =>
        await _context.WomenClothingImages.AddRangeAsync(images);

    public void RemoveSizes(IEnumerable<WomenClothingSizeSelection> sizes) =>
        _context.WomenClothingSizeSelections.RemoveRange(sizes);

    public async Task AddSizesAsync(IEnumerable<WomenClothingSizeSelection> sizes) =>
        await _context.WomenClothingSizeSelections.AddRangeAsync(sizes);

    public void RemoveColors(IEnumerable<WomenClothingColorSelection> colors) =>
        _context.WomenClothingColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<WomenClothingColorSelection> colors) =>
        await _context.WomenClothingColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
