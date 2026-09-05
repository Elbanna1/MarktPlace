using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.OnlineShopping;

namespace Persistence.Repositories;

public class GiftToyRepository : IGiftToyRepository
{
    private readonly AppDbContext _context;

    public GiftToyRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<GiftToy?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<GiftToy> query = _context.GiftToys.Include(x => x.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<GiftToy?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.GiftToys
            .Include(x => x.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<GiftToy> Items, int TotalCount)> GetPagedAsync(
        GiftToyFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.GiftToys.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.StoreName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                (x.OtherGiftType != null && EF.Functions.Like(x.OtherGiftType, $"%{search}%")));
        }

        if (!string.IsNullOrWhiteSpace(filter.StoreName))
        {
            var storeName = filter.StoreName.Trim();
            query = query.Where(x => EF.Functions.Like(x.StoreName, $"%{storeName}%"));
        }

        if (filter.GiftType is { } giftType)
            query = query.Where(x => x.GiftType == giftType);

        if (filter.SuitableFor is { } suitableFor)
            query = query.Where(x => x.SuitableFor == suitableFor);

        if (filter.GiftWrapping is { } giftWrapping)
            query = query.Where(x => x.GiftWrapping == giftWrapping);

        if (filter.DeliveryAvailable is { } deliveryAvailable)
            query = query.Where(x => x.DeliveryAvailable == deliveryAvailable);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<GiftToy>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            OnlineShoppingSorting.Apply(query, filter.SortBy, x => x.Price, x => x.CreatedAt, x => x.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<GiftToyTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default) =>
        _context.GiftToyTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<GiftToySuitableForLookup>> GetSuitableForAsync(CancellationToken cancellationToken = default) =>
        _context.GiftToySuitableFor.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(GiftToy entity) => await _context.GiftToys.AddAsync(entity);

    public void Update(GiftToy entity) => _context.GiftToys.Update(entity);

    public void RemoveImage(GiftToyImage image) => _context.GiftToyImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<GiftToyImage> images) =>
        await _context.GiftToyImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
