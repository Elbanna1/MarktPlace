using Shared.DTOs.Advertisements;
using Shared.DTOs.JobRequests;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IJobRequestService
{
    Task<JobRequestDetailsDto> CreateAsync(
        string userId,
        CreateJobRequestRequest request,
        UploadImageModel? profileImage,
        UploadImageModel? cvFile,
        UploadImageModel? introVideo,
        CancellationToken cancellationToken = default);

    Task<JobRequestDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateJobRequestRequest request,
        UploadImageModel? profileImage,
        UploadImageModel? cvFile,
        UploadImageModel? introVideo,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<JobRequestListItemDto>> GetListAsync(
        JobRequestFilterParams filter, CancellationToken cancellationToken = default);

    Task<JobRequestDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobFieldDto>> GetJobFieldsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobExperienceLevelDto>> GetExperienceLevelsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EducationLevelDto>> GetEducationLevelsAsync(CancellationToken cancellationToken = default);
}
