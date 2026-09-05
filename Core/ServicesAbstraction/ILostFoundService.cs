using Shared.DTOs.Advertisements;
using Shared.DTOs.LostFound;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ILostFoundService
{
    Task<LostFoundPostDetailsDto> CreateAsync(
        string userId,
        CreateLostFoundPostRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<LostFoundPostDetailsDto> UpdateAsync(
        string userId,
        Guid id,
        UpdateLostFoundPostRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<LostFoundPostListItemDto>> GetFeedAsync(
        LostFoundFilterParams filter, string? viewerUserId = null,
        CancellationToken cancellationToken = default);

    Task<LostFoundPostDetailsDto> GetByIdAsync(
        Guid id, string? viewerUserId, string? viewerIpAddress = null,
        CancellationToken cancellationToken = default);

    Task<LostFoundPostDetailsDto> MarkAsReturnedAsync(string userId, Guid id);

    Task<LikeResultDto> ToggleLikeAsync(string userId, Guid id);

    Task<LostFoundCommentDto> AddCommentAsync(string userId, Guid id, CreateCommentRequest request);

    Task<LostFoundCommentDto> UpdateCommentAsync(
        string userId, Guid id, Guid commentId, CreateCommentRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteCommentAsync(
        string userId, bool isAdmin, Guid id, Guid commentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LostFoundCommentDto>> GetCommentsAsync(
        Guid id, string? viewerUserId = null, bool isAdmin = false,
        CancellationToken cancellationToken = default);
}
