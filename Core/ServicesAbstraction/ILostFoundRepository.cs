using Domain.Entities;
using Shared.DTOs.LostFound;

namespace ServicesAbstraction;

public interface ILostFoundRepository
{
    Task<LostFoundPost?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<LostFoundPost?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<LostFoundPost> Items, int TotalCount)> GetPagedAsync(
        LostFoundFilterParams filter, CancellationToken cancellationToken = default);

    Task AddAsync(LostFoundPost post);

    void Update(LostFoundPost post);

    void RemoveImage(LostFoundImage image);

    Task AddImagesAsync(IEnumerable<LostFoundImage> images);

    Task<LostFoundLike?> GetLikeAsync(Guid postId, string userId);

    Task AddLikeAsync(LostFoundLike like);

    void RemoveLike(LostFoundLike like);

    Task<bool> HasLikedAsync(Guid postId, string userId, CancellationToken cancellationToken = default);

    Task<IReadOnlySet<Guid>> GetLikedIdsAsync(
        IReadOnlyCollection<Guid> postIds, string userId, CancellationToken cancellationToken = default);

    Task AddCommentAsync(LostFoundComment comment);

    Task<LostFoundComment?> GetCommentAsync(Guid commentId);

    Task<LostFoundComment?> GetCommentForUpdateAsync(
        Guid commentId, CancellationToken cancellationToken = default);

    void RemoveComment(LostFoundComment comment);

    Task<IReadOnlyList<LostFoundComment>> GetCommentsAsync(
        Guid postId, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync();
}
