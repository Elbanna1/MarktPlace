using Shared.DTOs.Advertisements;
using Shared.DTOs.Feedback;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IFeedbackService
{
    Task<FeedbackDetailsDto> CreateAsync(
        string userId,
        CreateFeedbackRequest request,
        IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<FeedbackListItemDto>> GetMyFeedbackAsync(
        string userId, FeedbackFilterParams filter, CancellationToken cancellationToken = default);

    Task<PaginatedResult<FeedbackListItemDto>> GetAllAsync(
        FeedbackFilterParams filter, CancellationToken cancellationToken = default);

    Task<FeedbackDetailsDto> GetByIdAsync(
        Guid id, string requesterUserId, bool isAdmin, CancellationToken cancellationToken = default);

    Task<FeedbackDetailsDto> UpdateStatusAsync(
        Guid id,
        string adminUserId,
        UpdateFeedbackStatusRequest request,
        CancellationToken cancellationToken = default);

    FeedbackMetadataDto GetMetadata();
}
