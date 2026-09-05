using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Persistence.Repositories;

public class CoinStampRepository : ICoinStampRepository
{
    private readonly AppDbContext _context;

    public CoinStampRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<CoinStamp?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<CoinStamp> query = _context.CoinStamps
            .Include(x => x.Images)
            .Include(x => x.Video);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<CoinStamp?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.CoinStamps
            .Include(x => x.Images)
            .Include(x => x.Video)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<CoinStamp> Items, int TotalCount)> GetPagedAsync(
        CoinStampFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.CoinStamps.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                EF.Functions.Like(x.ItemName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.SellerName))
        {
            var sellerName = filter.SellerName.Trim();
            query = query.Where(x => EF.Functions.Like(x.SellerName, $"%{sellerName}%"));
        }

        if (filter.ItemType is { } itemType)
            query = query.Where(x => x.ItemType == itemType);

        if (!string.IsNullOrWhiteSpace(filter.Country))
        {
            var country = filter.Country.Trim();
            query = query.Where(x => x.Country != null && EF.Functions.Like(x.Country, $"%{country}%"));
        }

        if (filter.IssueYear is { } issueYear)
            query = query.Where(x => x.IssueYear == issueYear);

        if (filter.IsOriginal is { } isOriginal)
            query = query.Where(x => x.IsOriginal == isOriginal);

        if (filter.IsRare is { } isRare)
            query = query.Where(x => x.IsRare == isRare);

        if (filter.Condition is { } condition)
            query = query.Where(x => x.Condition == condition);

        if (filter.Negotiable is { } negotiable)
            query = query.Where(x => x.Negotiable == negotiable);

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
            return (Array.Empty<CoinStamp>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            Sort(query, filter.SortBy),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .Include(x => x.Video)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    private static IQueryable<CoinStamp> Sort(IQueryable<CoinStamp> query, AntiqueSortBy sortBy) =>
        sortBy switch
        {
            AntiqueSortBy.Oldest => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            AntiqueSortBy.PriceAsc => query.OrderBy(x => x.Price).ThenByDescending(x => x.Id),
            AntiqueSortBy.PriceDesc => query.OrderByDescending(x => x.Price).ThenByDescending(x => x.Id),
            _ => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

    public Task<List<CoinStampItemTypeLookup>> GetItemTypesAsync(CancellationToken cancellationToken = default) =>
        _context.CoinStampItemTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<CoinStampMetalLookup>> GetMetalsAsync(CancellationToken cancellationToken = default) =>
        _context.CoinStampMetals.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<CoinStampConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default) =>
        _context.CoinStampConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(CoinStamp entity) => await _context.CoinStamps.AddAsync(entity);

    public void Update(CoinStamp entity) => _context.CoinStamps.Update(entity);

    public void RemoveImage(CoinStampImage image) => _context.CoinStampImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<CoinStampImage> images) =>
        await _context.CoinStampImages.AddRangeAsync(images);

    public void RemoveVideo(CoinStampVideo video) => _context.CoinStampVideos.Remove(video);

    public async Task AddVideoAsync(CoinStampVideo video) => await _context.CoinStampVideos.AddAsync(video);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
