using Persistence.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.LostFound;
using Shared.Enums;

namespace Persistence.Repositories;

public class LostFoundRepository : ILostFoundRepository
{
    private readonly AppDbContext _context;

    public LostFoundRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<LostFoundPost?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false)
    {
        IQueryable<LostFoundPost> query = _context.LostFoundPosts.Include(p => p.Images);

        query = query.VisibleToViewer(includeUnmoderated, _context.ViewerUserId);

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<LostFoundPost?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default) =>
        _context.LostFoundPosts
            .Include(p => p.Images)
            .IncludingUnmoderated()
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId, cancellationToken);

    public async Task<(IReadOnlyList<LostFoundPost> Items, int TotalCount)> GetPagedAsync(
        LostFoundFilterParams filter, CancellationToken cancellationToken = default)
    {
        var query = _context.LostFoundPosts.AsNoTracking().AsQueryable();

        var postType = filter.PostType
            ?? throw new ArgumentException(
                "A post type is required: ضايع مني and لقيت are separate feeds.", nameof(filter));

        query = query.Where(p => p.PostType == postType);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(p =>
                EF.Functions.Like(p.ItemName, $"%{term}%") ||
                EF.Functions.Like(p.Center, $"%{term}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var city = filter.City.Trim();
            query = query.Where(p => p.Center == city);
        }

        if (filter.Date is { } date)
        {
            query = postType == PostType.Lost
                ? query.Where(p => p.LostDate == date)
                : query.Where(p => p.FoundDate == date);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
            return (Array.Empty<LostFoundPost>(), 0);

        var items = await PagedListingQuery.ToPageAsync(
            query
                .OrderByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.Id),
            filter.PageIndex,
            filter.PageSize,
            keyed => keyed
                .Include(p => p.Images)
                .AsSplitQuery(),
            cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(LostFoundPost post) =>
        await _context.LostFoundPosts.AddAsync(post);

    public void Update(LostFoundPost post) =>
        _context.LostFoundPosts.Update(post);

    public void RemoveImage(LostFoundImage image) =>
        _context.LostFoundImages.Remove(image);

    public async Task AddImagesAsync(IEnumerable<LostFoundImage> images) =>
        await _context.LostFoundImages.AddRangeAsync(images);

    public Task<LostFoundLike?> GetLikeAsync(Guid postId, string userId) =>
        _context.LostFoundLikes
            .IncludingUnmoderatedParent()
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

    public async Task AddLikeAsync(LostFoundLike like) =>
        await _context.LostFoundLikes.AddAsync(like);

    public void RemoveLike(LostFoundLike like) =>
        _context.LostFoundLikes.Remove(like);

    public Task<bool> HasLikedAsync(
        Guid postId, string userId, CancellationToken cancellationToken = default) =>
        _context.LostFoundLikes
            .IncludingUnmoderatedParent()
            .AnyAsync(l => l.PostId == postId && l.UserId == userId, cancellationToken);

    public async Task<IReadOnlySet<Guid>> GetLikedIdsAsync(
        IReadOnlyCollection<Guid> postIds, string userId, CancellationToken cancellationToken = default)
    {
        if (postIds.Count == 0)
            return new HashSet<Guid>();

        var liked = await _context.LostFoundLikes
            .AsNoTracking()
            .Where(l => l.UserId == userId && postIds.Contains(l.PostId))
            .Select(l => l.PostId)
            .ToListAsync(cancellationToken);

        return liked.ToHashSet();
    }

    public async Task AddCommentAsync(LostFoundComment comment) =>
        await _context.LostFoundComments.AddAsync(comment);

    public Task<LostFoundComment?> GetCommentAsync(Guid commentId) =>
        _context.LostFoundComments
            .Include(c => c.User)
            .IncludingUnmoderatedParent()
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == commentId);

    public Task<LostFoundComment?> GetCommentForUpdateAsync(
        Guid commentId, CancellationToken cancellationToken = default) =>
        _context.LostFoundComments
            .IncludingUnmoderatedParent()
            .FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);

    public void RemoveComment(LostFoundComment comment) =>
        _context.LostFoundComments.Remove(comment);

    public async Task<IReadOnlyList<LostFoundComment>> GetCommentsAsync(
        Guid postId, CancellationToken cancellationToken = default) =>
        await _context.LostFoundComments
            .AsNoTracking()
            .Include(c => c.User)
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.CreatedAt)
            .ThenByDescending(c => c.Id)
            .Take(LostFoundConstants.MaxCommentsReturned)
            .ToListAsync(cancellationToken);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
