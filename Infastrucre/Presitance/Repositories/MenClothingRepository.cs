using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Clothing;

namespace Persistence.Repositories;

public class MenClothingRepository : IMenClothingRepository
{
    private readonly AppDbContext _context;

    public MenClothingRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<MenClothing?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<MenClothing> query = _context.MenClothings
            .Include(x => x.Images)
            .Include(x => x.Sizes)
            .Include(x => x.Colors);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<MenClothing?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.MenClothings
            .Include(x => x.Images)
            .Include(x => x.Sizes)
            .Include(x => x.Colors)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<MenClothing> Items, int TotalCount)> GetPagedAsync(
        MenClothingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.MenClothings.AsNoTracking().AsQueryable();

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
            return (Array.Empty<MenClothing>(), 0);

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

    public Task<List<MenClothingTypeLookup>> GetClothingTypesAsync(CancellationToken cancellationToken = default) =>
        _context.MenClothingTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<MenClothingBrandLookup>> GetBrandsAsync(CancellationToken cancellationToken = default) =>
        _context.MenClothingBrands.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<MenClothingSizeLookup>> GetSizesAsync(CancellationToken cancellationToken = default) =>
        _context.MenClothingSizes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<MenClothingColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.MenClothingColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<MenClothingConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default) =>
        _context.MenClothingConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<MenClothingSellingMethodLookup>> GetSellingMethodsAsync(CancellationToken cancellationToken = default) =>
        _context.MenClothingSellingMethods.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(MenClothing entity) => await _context.MenClothings.AddAsync(entity);

    public void Update(MenClothing entity) => _context.MenClothings.Update(entity);

    public void RemoveImage(MenClothingImage image) => _context.MenClothingImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<MenClothingImage> images) =>
        await _context.MenClothingImages.AddRangeAsync(images);

    public void RemoveSizes(IEnumerable<MenClothingSizeSelection> sizes) =>
        _context.MenClothingSizeSelections.RemoveRange(sizes);

    public async Task AddSizesAsync(IEnumerable<MenClothingSizeSelection> sizes) =>
        await _context.MenClothingSizeSelections.AddRangeAsync(sizes);

    public void RemoveColors(IEnumerable<MenClothingColorSelection> colors) =>
        _context.MenClothingColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<MenClothingColorSelection> colors) =>
        await _context.MenClothingColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
