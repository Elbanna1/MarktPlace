using Shared.DTOs.Advertisements;
using Shared.DTOs.Charity;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IRescueService
{
    Task<RescueDetailsDto> CreateAsync(
        string userId, CreateRescueRequest request, IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default);

    Task<RescueDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateRescueRequest request,
        IReadOnlyList<UploadImageModel> newImages, CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin, CancellationToken cancellationToken = default);

    Task<PaginatedResult<RescueListItemDto>> GetListAsync(
        RescueFilterParams filter, CancellationToken cancellationToken = default);

    Task<RescueDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IBloodRequestService
{
    Task<BloodRequestDetailsDto> CreateAsync(
        string userId, CreateBloodRequestRequest request, IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default);

    Task<BloodRequestDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateBloodRequestRequest request,
        IReadOnlyList<UploadImageModel> newImages, CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin, CancellationToken cancellationToken = default);

    Task<PaginatedResult<BloodRequestListItemDto>> GetListAsync(
        BloodRequestFilterParams filter, CancellationToken cancellationToken = default);

    Task<BloodRequestDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IAskConsultService
{
    Task<AskConsultDetailsDto> CreateAsync(
        string userId, CreateAskConsultRequest request, IReadOnlyList<UploadImageModel> images,
        CancellationToken cancellationToken = default);

    Task<AskConsultDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateAskConsultRequest request,
        IReadOnlyList<UploadImageModel> newImages, CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin, CancellationToken cancellationToken = default);

    Task<PaginatedResult<AskConsultListItemDto>> GetListAsync(
        AskConsultFilterParams filter, string? viewerUserId = null,
        CancellationToken cancellationToken = default);

    Task<AskConsultDetailsDto> GetByIdAsync(
        Guid id, string? viewerUserId = null, CancellationToken cancellationToken = default);

    Task<AskConsultLikeResultDto> ToggleLikeAsync(
        string userId, Guid id, CancellationToken cancellationToken = default);

    Task<AskConsultCommentDto> AddCommentAsync(
        string userId, Guid id, CreateAskConsultCommentRequest request,
        CancellationToken cancellationToken = default);

    Task<AskConsultCommentDto> UpdateCommentAsync(
        string userId, Guid id, Guid commentId, CreateAskConsultCommentRequest request,
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<AskConsultCommentDto>> GetCommentsAsync(
        Guid id, int pageIndex, int pageSize, string? viewerUserId = null, bool isAdmin = false,
        CancellationToken cancellationToken = default);

    Task DeleteCommentAsync(
        string userId, bool isAdmin, Guid id, Guid commentId, CancellationToken cancellationToken = default);
}
