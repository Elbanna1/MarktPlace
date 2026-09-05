using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Persistence.Repositories;

public class AntiqueRepository : IAntiqueRepository
{
    private readonly AppDbContext _context;

    public AntiqueRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Antique?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<Antique> query = _context.Antiques
            .Include(x => x.Images)
            .Include(x => x.Video);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Antique?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.Antiques
            .Include(x => x.Images)
            .Include(x => x.Video)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<Antique> Items, int TotalCount)> GetPagedAsync(
        AntiqueFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Antiques.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.SellerName, $"%{search}%") ||
                EF.Functions.Like(x.AntiqueName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.SellerName))
        {
            var sellerName = filter.SellerName.Trim();
            query = query.Where(x => EF.Functions.Like(x.SellerName, $"%{sellerName}%"));
        }

        if (filter.AntiqueType is { } antiqueType)
            query = query.Where(x => x.AntiqueType == antiqueType);

        if (filter.ManufactureYear is { } manufactureYear)
            query = query.Where(x => x.ManufactureYear == manufactureYear);

        if (!string.IsNullOrWhiteSpace(filter.CountryOfOrigin))
        {
            var country = filter.CountryOfOrigin.Trim();
            query = query.Where(x => x.CountryOfOrigin != null &&
                                     EF.Functions.Like(x.CountryOfOrigin, $"%{country}%"));
        }

        if (filter.Originality is { } originality)
            query = query.Where(x => x.Originality == originality);

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
            return (Array.Empty<Antique>(), 0);

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

    private static IQueryable<Antique> Sort(IQueryable<Antique> query, AntiqueSortBy sortBy) =>
        sortBy switch
        {
            AntiqueSortBy.Oldest => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            AntiqueSortBy.PriceAsc => query.OrderBy(x => x.Price).ThenByDescending(x => x.Id),
            AntiqueSortBy.PriceDesc => query.OrderByDescending(x => x.Price).ThenByDescending(x => x.Id),
            _ => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

    public Task<List<AntiqueTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default) =>
        _context.AntiqueTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<AntiqueMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default) =>
        _context.AntiqueMaterials.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<AntiqueConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default) =>
        _context.AntiqueConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<AntiqueWorkingStatusLookup>> GetWorkingStatusesAsync(CancellationToken cancellationToken = default) =>
        _context.AntiqueWorkingStatuses.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<AntiqueOriginalityLookup>> GetOriginalitiesAsync(CancellationToken cancellationToken = default) =>
        _context.AntiqueOriginalities.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(Antique entity) => await _context.Antiques.AddAsync(entity);

    public void Update(Antique entity) => _context.Antiques.Update(entity);

    public void RemoveImage(AntiqueImage image) => _context.AntiqueImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<AntiqueImage> images) =>
        await _context.AntiqueImages.AddRangeAsync(images);

    public void RemoveVideo(AntiqueVideo video) => _context.AntiqueVideos.Remove(video);

    public async Task AddVideoAsync(AntiqueVideo video) => await _context.AntiqueVideos.AddAsync(video);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
