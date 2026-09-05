using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Data;
using ServicesAbstraction;
using Shared.DTOs.Charity;
using Shared.Enums;

namespace Persistence.Repositories;

public abstract class CharityRepositoryBase<TListing, TFilter> : ICharityRepository<TListing, TFilter>
    where TListing : CharityListing
    where TFilter : CharityFilterParamsBase
{
    protected readonly AppDbContext Context;

    protected CharityRepositoryBase(AppDbContext context)
    {
        Context = context;
    }

    protected abstract DbSet<TListing> Set { get; }

    protected abstract IQueryable<TListing> Include(IQueryable<TListing> query);

    protected abstract IQueryable<TListing> Filter(IQueryable<TListing> query, TFilter filter);

    public Task<TListing?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default,
        bool includeUnmoderated = false)
    {
        var query = Include(Set);

        query = query.VisibleToViewer(includeUnmoderated, Context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<TListing?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        Include(Set)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<TListing> Items, int TotalCount)> GetPagedAsync(
        TFilter filter, CancellationToken cancellationToken = default)
    {
        var query = Filter(Include(Set).AsNoTracking(), filter);

        if (filter.HasLocation is { } hasLocation)
        {
            query = hasLocation
                ? query.Where(x => x.Latitude != null && x.Longitude != null)
                : query.Where(x => x.Latitude == null || x.Longitude == null);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = ApplySearch(query, filter.Search.Trim());

        var total = await query.CountAsync(cancellationToken);

        query = Sort(query, filter.SortBy);

        var items = await query
            .Skip((filter.PageIndex - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    protected abstract IQueryable<TListing> ApplySearch(IQueryable<TListing> query, string search);

    protected virtual IQueryable<TListing> Sort(IQueryable<TListing> query, CharitySortBy sortBy) =>
        sortBy switch
        {
            CharitySortBy.Oldest => query.OrderBy(x => x.CreatedAt),
            CharitySortBy.MostViewed => query.OrderByDescending(x => x.ViewCount).ThenByDescending(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };

    public async Task AddAsync(TListing listing, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(listing, cancellationToken);

    public void Update(TListing listing) => Set.Update(listing);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        Context.SaveChangesAsync(cancellationToken);
}

public class RescueRepository : CharityRepositoryBase<Rescue, RescueFilterParams>, IRescueRepository
{
    public RescueRepository(AppDbContext context) : base(context) { }

    protected override DbSet<Rescue> Set => Context.Rescues;

    protected override IQueryable<Rescue> Include(IQueryable<Rescue> query) =>
        query.Include(x => x.Images).Include(x => x.User);

    protected override IQueryable<Rescue> Filter(IQueryable<Rescue> query, RescueFilterParams filter) => query;

    protected override IQueryable<Rescue> ApplySearch(IQueryable<Rescue> query, string search) =>
        query.Where(x =>
            EF.Functions.Like(x.RescuerName, $"%{search}%") ||
            EF.Functions.Like(x.Address, $"%{search}%") ||
            EF.Functions.Like(x.Details, $"%{search}%"));

    public async Task AddImagesAsync(
        IEnumerable<RescueImage> images, CancellationToken cancellationToken = default) =>
        await Context.RescueImages.AddRangeAsync(images, cancellationToken);

    public void RemoveImage(RescueImage image) => Context.RescueImages.Remove(image);
}

public class BloodRequestRepository
    : CharityRepositoryBase<BloodRequest, BloodRequestFilterParams>, IBloodRequestRepository
{
    public BloodRequestRepository(AppDbContext context) : base(context) { }

    protected override DbSet<BloodRequest> Set => Context.BloodRequests;

    protected override IQueryable<BloodRequest> Include(IQueryable<BloodRequest> query) =>
        query.Include(x => x.Images).Include(x => x.User);

    protected override IQueryable<BloodRequest> Filter(
        IQueryable<BloodRequest> query, BloodRequestFilterParams filter)
    {
        if (filter.BloodGroup is { } group)
            query = query.Where(x => x.BloodGroup == group);

        if (!string.IsNullOrWhiteSpace(filter.Center))
            query = query.Where(x => x.Center == filter.Center);

        return query;
    }

    protected override IQueryable<BloodRequest> ApplySearch(IQueryable<BloodRequest> query, string search) =>
        query.Where(x =>
            EF.Functions.Like(x.RequesterName, $"%{search}%") ||
            EF.Functions.Like(x.HospitalName, $"%{search}%") ||
            EF.Functions.Like(x.Address, $"%{search}%") ||
            EF.Functions.Like(x.Details, $"%{search}%"));

    public async Task AddImagesAsync(
        IEnumerable<BloodRequestImage> images, CancellationToken cancellationToken = default) =>
        await Context.BloodRequestImages.AddRangeAsync(images, cancellationToken);

    public void RemoveImage(BloodRequestImage image) => Context.BloodRequestImages.Remove(image);
}

public class AskConsultRepository
    : CharityRepositoryBase<AskConsult, AskConsultFilterParams>, IAskConsultRepository
{
    public AskConsultRepository(AppDbContext context) : base(context) { }

    protected override DbSet<AskConsult> Set => Context.AskConsults;

    protected override IQueryable<AskConsult> Include(IQueryable<AskConsult> query) =>
        query.Include(x => x.Images).Include(x => x.User);

    protected override IQueryable<AskConsult> Filter(
        IQueryable<AskConsult> query, AskConsultFilterParams filter)
    {
        if (filter.Category is { } category)
            query = query.Where(x => x.Category == category);

        return query;
    }

    protected override IQueryable<AskConsult> ApplySearch(IQueryable<AskConsult> query, string search) =>
        query.Where(x =>
            EF.Functions.Like(x.Title, $"%{search}%") ||
            EF.Functions.Like(x.Question, $"%{search}%") ||
            EF.Functions.Like(x.AskerName, $"%{search}%") ||
            (x.OtherCategory != null && EF.Functions.Like(x.OtherCategory, $"%{search}%")));

    protected override IQueryable<AskConsult> Sort(IQueryable<AskConsult> query, CharitySortBy sortBy) =>
        sortBy == CharitySortBy.MostLiked
            ? query.OrderByDescending(x => x.LikesCount).ThenByDescending(x => x.CreatedAt)
            : base.Sort(query, sortBy);

    public async Task AddImagesAsync(
        IEnumerable<AskConsultImage> images, CancellationToken cancellationToken = default) =>
        await Context.AskConsultImages.AddRangeAsync(images, cancellationToken);

    public void RemoveImage(AskConsultImage image) => Context.AskConsultImages.Remove(image);

    public Task<AskConsultLike?> GetLikeAsync(
        Guid listingId, string userId, CancellationToken cancellationToken = default) =>
        Context.AskConsultLikes
            .IncludingUnmoderatedParent()
            .FirstOrDefaultAsync(x => x.AskConsultId == listingId && x.UserId == userId, cancellationToken);

    public async Task AddLikeAsync(AskConsultLike like, CancellationToken cancellationToken = default) =>
        await Context.AskConsultLikes.AddAsync(like, cancellationToken);

    public void RemoveLike(AskConsultLike like) => Context.AskConsultLikes.Remove(like);

    public async Task<IReadOnlySet<Guid>> GetLikedIdsAsync(
        IReadOnlyCollection<Guid> listingIds, string userId, CancellationToken cancellationToken = default)
    {
        if (listingIds.Count == 0)
            return new HashSet<Guid>();

        var liked = await Context.AskConsultLikes
            .AsNoTracking()
            .Where(x => x.UserId == userId && listingIds.Contains(x.AskConsultId))
            .Select(x => x.AskConsultId)
            .ToListAsync(cancellationToken);

        return liked.ToHashSet();
    }

    public async Task AddCommentAsync(
        AskConsultComment comment, CancellationToken cancellationToken = default) =>
        await Context.AskConsultComments.AddAsync(comment, cancellationToken);

    public Task<AskConsultComment?> GetCommentAsync(
        Guid commentId, CancellationToken cancellationToken = default) =>
        Context.AskConsultComments
            .Include(x => x.User)
            .IncludingUnmoderatedParent()
            .FirstOrDefaultAsync(x => x.Id == commentId, cancellationToken);

    public async Task<(IReadOnlyList<AskConsultComment> Items, int TotalCount)> GetCommentsAsync(
        Guid listingId, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = Context.AskConsultComments
            .AsNoTracking()
            .Where(x => x.AskConsultId == listingId);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(x => x.User)
            .OrderBy(x => x.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public void RemoveComment(AskConsultComment comment) => Context.AskConsultComments.Remove(comment);
}
