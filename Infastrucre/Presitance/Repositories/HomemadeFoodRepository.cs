using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.OnlineShopping;

namespace Persistence.Repositories;

public class HomemadeFoodRepository : IHomemadeFoodRepository
{
    private readonly AppDbContext _context;

    public HomemadeFoodRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<HomemadeFood?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<HomemadeFood> query = _context.HomemadeFoods
            .Include(x => x.Images)
            .Include(x => x.DeliveryAreas);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<HomemadeFood?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.HomemadeFoods
            .Include(x => x.Images)
            .Include(x => x.DeliveryAreas)
            .AsSplitQuery()
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<HomemadeFood> Items, int TotalCount)> GetPagedAsync(
        HomemadeFoodFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.HomemadeFoods.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.ProjectName, $"%{search}%") ||
                EF.Functions.Like(x.Title, $"%{search}%") ||
                EF.Functions.Like(x.Description, $"%{search}%") ||
                (x.OtherSection != null && EF.Functions.Like(x.OtherSection, $"%{search}%")) ||
                (x.Ingredients != null && EF.Functions.Like(x.Ingredients, $"%{search}%")));
        }

        if (!string.IsNullOrWhiteSpace(filter.ProjectName))
        {
            var projectName = filter.ProjectName.Trim();
            query = query.Where(x => EF.Functions.Like(x.ProjectName, $"%{projectName}%"));
        }

        if (filter.Section is { } section)
            query = query.Where(x => x.Section == section);

        if (filter.DeliveryAvailable is { } deliveryAvailable)
            query = query.Where(x => x.DeliveryAvailable == deliveryAvailable);

        if (filter.DeliveryArea is { } deliveryArea)
            query = query.Where(x => x.DeliveryAreas.Any(selection => selection.DeliveryArea == deliveryArea));

        if (filter.PreparedOnDemand is { } preparedOnDemand)
            query = query.Where(x => x.PreparedOnDemand == preparedOnDemand);

        if (filter.PriceFrom is { } priceFrom)
            query = query.Where(x => x.Price >= priceFrom);

        if (filter.PriceTo is { } priceTo)
            query = query.Where(x => x.Price <= priceTo);

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<HomemadeFood>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            OnlineShoppingSorting.Apply(query, filter.SortBy, x => x.Price, x => x.CreatedAt, x => x.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(x => x.Images)
                .Include(x => x.DeliveryAreas)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public Task<List<HomemadeFoodSectionLookup>> GetSectionsAsync(CancellationToken cancellationToken = default) =>
        _context.HomemadeFoodSections.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<List<HomemadeFoodDeliveryAreaLookup>> GetDeliveryAreasAsync(
        CancellationToken cancellationToken = default) =>
        _context.HomemadeFoodDeliveryAreas.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public async Task AddAsync(HomemadeFood entity) => await _context.HomemadeFoods.AddAsync(entity);

    public void Update(HomemadeFood entity) => _context.HomemadeFoods.Update(entity);

    public void RemoveImage(HomemadeFoodImage image) => _context.HomemadeFoodImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<HomemadeFoodImage> images) =>
        await _context.HomemadeFoodImages.AddRangeAsync(images);

    public void RemoveDeliveryAreas(IEnumerable<HomemadeFoodDeliveryAreaSelection> areas) =>
        _context.HomemadeFoodDeliveryAreaSelections.RemoveRange(areas);

    public async Task AddDeliveryAreasAsync(IEnumerable<HomemadeFoodDeliveryAreaSelection> areas) =>
        await _context.HomemadeFoodDeliveryAreaSelections.AddRangeAsync(areas);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
