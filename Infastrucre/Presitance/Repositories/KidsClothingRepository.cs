using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Clothing;

namespace Persistence.Repositories;

public class KidsClothingRepository : IKidsClothingRepository
{
    private readonly AppDbContext _context;

    public KidsClothingRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<KidsClothing?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<KidsClothing> query = _context.KidsClothings
            .Include(x => x.Images)
            .Include(x => x.Sizes)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<KidsClothing?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.KidsClothings
            .Include(x => x.Images)
            .Include(x => x.Sizes)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<KidsClothing> Items, int TotalCount)> GetPagedAsync(
        KidsClothingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.KidsClothings.AsNoTracking().AsQueryable();

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
            return (Array.Empty<KidsClothing>(), 0);

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

    public Task<List<KidsClothingTypeLookup>> GetClothingTypesAsync(CancellationToken cancellationToken = default) =>
        _context.KidsClothingTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<KidsClothingBrandLookup>> GetBrandsAsync(CancellationToken cancellationToken = default) =>
        _context.KidsClothingBrands.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<KidsClothingSizeLookup>> GetSizesAsync(CancellationToken cancellationToken = default) =>
        _context.KidsClothingSizes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<KidsClothingColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.KidsClothingColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<KidsClothingConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default) =>
        _context.KidsClothingConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<KidsClothingSellingMethodLookup>> GetSellingMethodsAsync(CancellationToken cancellationToken = default) =>
        _context.KidsClothingSellingMethods.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(KidsClothing entity) => await _context.KidsClothings.AddAsync(entity);

    public void Update(KidsClothing entity) => _context.KidsClothings.Update(entity);

    public void RemoveImage(KidsClothingImage image) => _context.KidsClothingImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<KidsClothingImage> images) =>
        await _context.KidsClothingImages.AddRangeAsync(images);

    public void RemoveSizes(IEnumerable<KidsClothingSizeSelection> sizes) =>
        _context.KidsClothingSizeSelections.RemoveRange(sizes);

    public async Task AddSizesAsync(IEnumerable<KidsClothingSizeSelection> sizes) =>
        await _context.KidsClothingSizeSelections.AddRangeAsync(sizes);

    public void RemoveColors(IEnumerable<KidsClothingColorSelection> colors) =>
        _context.KidsClothingColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<KidsClothingColorSelection> colors) =>
        await _context.KidsClothingColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
