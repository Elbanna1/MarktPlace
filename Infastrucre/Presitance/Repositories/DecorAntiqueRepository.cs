using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Antiques;
using Shared.Enums;

namespace Persistence.Repositories;

public class DecorAntiqueRepository : IDecorAntiqueRepository
{
    private readonly AppDbContext _context;

    public DecorAntiqueRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<DecorAntique?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<DecorAntique> query = _context.DecorAntiques
            .Include(x => x.Images)
            .Include(x => x.Video);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<DecorAntique?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.DecorAntiques
            .Include(x => x.Images)
            .Include(x => x.Video)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<DecorAntique> Items, int TotalCount)> GetPagedAsync(
        DecorAntiqueFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.DecorAntiques.AsNoTracking().AsQueryable();

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

        if (filter.Material is { } material)
            query = query.Where(x => x.Material == material);

        if (filter.Condition is { } condition)
            query = query.Where(x => x.Condition == condition);

        if (filter.Originality is { } originality)
            query = query.Where(x => x.Originality == originality);

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
            return (Array.Empty<DecorAntique>(), 0);

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

    private static IQueryable<DecorAntique> Sort(IQueryable<DecorAntique> query, AntiqueSortBy sortBy) =>
        sortBy switch
        {
            AntiqueSortBy.Oldest => query.OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),
            AntiqueSortBy.PriceAsc => query.OrderBy(x => x.Price).ThenByDescending(x => x.Id),
            AntiqueSortBy.PriceDesc => query.OrderByDescending(x => x.Price).ThenByDescending(x => x.Id),
            _ => query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id)
        };

    public Task<List<DecorAntiqueItemTypeLookup>> GetItemTypesAsync(CancellationToken cancellationToken = default) =>
        _context.DecorAntiqueItemTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<DecorAntiqueMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default) =>
        _context.DecorAntiqueMaterials.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<DecorAntiqueConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default) =>
        _context.DecorAntiqueConditions.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<DecorAntiqueOriginalityLookup>> GetOriginalitiesAsync(CancellationToken cancellationToken = default) =>
        _context.DecorAntiqueOriginalities.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(DecorAntique entity) => await _context.DecorAntiques.AddAsync(entity);

    public void Update(DecorAntique entity) => _context.DecorAntiques.Update(entity);

    public void RemoveImage(DecorAntiqueImage image) => _context.DecorAntiqueImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<DecorAntiqueImage> images) =>
        await _context.DecorAntiqueImages.AddRangeAsync(images);

    public void RemoveVideo(DecorAntiqueVideo video) => _context.DecorAntiqueVideos.Remove(video);

    public async Task AddVideoAsync(DecorAntiqueVideo video) =>
        await _context.DecorAntiqueVideos.AddAsync(video);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
