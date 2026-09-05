using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Persistence.Repositories;

public class PaintingRepository : IPaintingRepository
{
    private readonly AppDbContext _context;

    public PaintingRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Painting?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Painting> query = _context.Paintings
            .Include(x => x.Images)
            .Include(x => x.Video);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Painting?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Paintings
            .Include(x => x.Images)
            .Include(x => x.Video)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Painting> Items, int TotalCount)> GetPagedAsync(
        PaintingFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Paintings.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                EF.Functions.Like(x.ArtistName, $"%{search}%") ||
                EF.Functions.Like(x.PaintingName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.SellerName))
        {
            var sellerName = filter.SellerName.Trim();
            query = query.Where(x => EF.Functions.Like(x.SellerName, $"%{sellerName}%"));
        }

        if (filter.PaintingType is { } paintingType)
            query = query.Where(x => x.PaintingType == paintingType);

        if (!string.IsNullOrWhiteSpace(filter.ArtistName))
        {
            var artistName = filter.ArtistName.Trim();
            query = query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));
        }

        if (filter.Originality is { } originality)
            query = query.Where(x => x.Originality == originality);

        if (filter.Framed is { } framed)
            query = query.Where(x => x.Framed == framed);

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
            return (Array.Empty<Painting>(), 0);

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

    private static IQueryable<Painting> Sort(IQueryable<Painting> query, AntiqueSortBy sortBy) =>
        sortBy switch
        {
            AntiqueSortBy.Oldest => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            AntiqueSortBy.PriceAsc => query.OrderBy(x => x.Price).ThenByDescending(x => x.Id),
            AntiqueSortBy.PriceDesc => query.OrderByDescending(x => x.Price).ThenByDescending(x => x.Id),
            _ => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

    public Task<List<PaintingTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default) =>
        _context.PaintingTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PaintingMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default) =>
        _context.PaintingMaterials.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<PaintingOriginalityLookup>> GetOriginalitiesAsync(CancellationToken cancellationToken = default) =>
        _context.PaintingOriginalities.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Painting entity) => await _context.Paintings.AddAsync(entity);

    public void Update(Painting entity) => _context.Paintings.Update(entity);

    public void RemoveImage(PaintingImage image) => _context.PaintingImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<PaintingImage> images) =>
        await _context.PaintingImages.AddRangeAsync(images);

    public void RemoveVideo(PaintingVideo video) => _context.PaintingVideos.Remove(video);

    public async Task AddVideoAsync(PaintingVideo video) => await _context.PaintingVideos.AddAsync(video);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
