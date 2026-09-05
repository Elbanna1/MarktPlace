using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.WholesaleTraders;

namespace Persistence.Repositories;

public class WholesaleTraderRepository : IWholesaleTraderRepository
{
    private readonly AppDbContext _context;

    public WholesaleTraderRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<WholesaleTrader?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<WholesaleTrader> query = _context.WholesaleTraders.Include(t => t.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<WholesaleTrader?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.WholesaleTraders
            .Include(t => t.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<WholesaleTrader> Items, int TotalCount)> GetPagedAsync(
        WholesaleTraderFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.WholesaleTraders.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(t =>
                EF.Functions.Like(t.TraderName, $"%{search}%") ||
                EF.Functions.Like(t.ProductsName, $"%{search}%") ||
                EF.Functions.Like(t.ProductDetails, $"%{search}%") ||
                EF.Functions.Like(t.Title, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.TraderName))
        {
            var traderName = filter.TraderName.Trim();
            query = query.Where(t => EF.Functions.Like(t.TraderName, $"%{traderName}%"));
        }

        if (filter.TradeType is { } tradeType)
            query = query.Where(t => t.TradeType == tradeType);

        if (!string.IsNullOrWhiteSpace(filter.ProductsName))
        {
            var productsName = filter.ProductsName.Trim();
            query = query.Where(t => EF.Functions.Like(t.ProductsName, $"%{productsName}%"));
        }

        if (filter.SaleType is { } saleType)
            query = query.Where(t => t.SaleType == saleType);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<WholesaleTrader>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(t => t.CreatedAt)
                .ThenByDescending(t => t.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(t => t.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<WholesaleTradeTypeLookup>> GetTradeTypesAsync(
        CancellationToken cancellationToken = default) =>
        _context.WholesaleTradeTypes
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(WholesaleTrader trader) =>
        await _context.WholesaleTraders.AddAsync(trader);

    public void Update(WholesaleTrader trader) =>
        _context.WholesaleTraders.Update(trader);

    public void RemoveImage(WholesaleTraderImage image) =>
        _context.WholesaleTraderImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<WholesaleTraderImage> images) =>
        await _context.WholesaleTraderImages.AddRangeAsync(images);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
