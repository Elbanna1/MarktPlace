using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Persistence.Repositories;

public class HandmadeRepository : IHandmadeRepository
{
    private readonly AppDbContext _context;

    public HandmadeRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Handmade?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Handmade> query = _context.Handmades
            .Include(x => x.Colors)
            .Include(x => x.Images)
            .Include(x => x.Video);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Handmade?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Handmades
            .Include(x => x.Colors)
            .Include(x => x.Images)
            .Include(x => x.Video)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Handmade> Items, int TotalCount)> GetPagedAsync(
        HandmadeFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Handmades.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                EF.Functions.Like(x.ProductName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.SellerName))
        {
            var sellerName = filter.SellerName.Trim();
            query = query.Where(x => EF.Functions.Like(x.SellerName, $"%{sellerName}%"));
        }

        if (filter.HandmadeType is { } handmadeType)
            query = query.Where(x => x.HandmadeType == handmadeType);

        if (filter.IsFullyHandmade is { } isFullyHandmade)
            query = query.Where(x => x.IsFullyHandmade == isFullyHandmade);

        if (filter.CustomOrder is { } customOrder)
            query = query.Where(x => x.CustomOrder == customOrder);

        if (filter.Color is { } color)
            query = query.Where(x => x.Colors.Any(c => c.Color == color));

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
            return (Array.Empty<Handmade>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            Sort(query, filter.SortBy),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Colors)
                .Include(x => x.Images)
                .Include(x => x.Video)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    private static IQueryable<Handmade> Sort(IQueryable<Handmade> query, AntiqueSortBy sortBy) =>
        sortBy switch
        {
            AntiqueSortBy.Oldest => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            AntiqueSortBy.PriceAsc => query.OrderBy(x => x.Price).ThenByDescending(x => x.Id),
            AntiqueSortBy.PriceDesc => query.OrderByDescending(x => x.Price).ThenByDescending(x => x.Id),
            _ => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

    public Task<List<HandmadeTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default) =>
        _context.HandmadeTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HandmadeColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default) =>
        _context.HandmadeColors.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Handmade entity) => await _context.Handmades.AddAsync(entity);

    public void Update(Handmade entity) => _context.Handmades.Update(entity);

    public void RemoveImage(HandmadeImage image) => _context.HandmadeImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<HandmadeImage> images) =>
        await _context.HandmadeImages.AddRangeAsync(images);

    public void RemoveVideo(HandmadeVideo video) => _context.HandmadeVideos.Remove(video);

    public async Task AddVideoAsync(HandmadeVideo video) => await _context.HandmadeVideos.AddAsync(video);

    public void RemoveColors(IEnumerable<HandmadeColorSelection> colors) =>
        _context.HandmadeColorSelections.RemoveRange(colors);

    public async Task AddColorsAsync(IEnumerable<HandmadeColorSelection> colors) =>
        await _context.HandmadeColorSelections.AddRangeAsync(colors);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
