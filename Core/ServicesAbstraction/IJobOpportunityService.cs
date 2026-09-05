using Shared.DTOs.Advertisements;
using Shared.DTOs.JobOpportunities;
using Shared.DTOs.JobRequests;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IJobOpportunityService
{
    Task<JobOpportunityDetailsDto> CreateAsync(
        string userId,
        CreateJobOpportunityRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? logo,
        CancellationToken cancellationToken = default);

    Task<JobOpportunityDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateJobOpportunityRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? logo,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<JobOpportunityListItemDto>> GetListAsync(
        JobOpportunityFilterParams filter, CancellationToken cancellationToken = default);

    Task<JobOpportunityDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobFieldDto>> GetJobFieldsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<JobExperienceLevelDto>> GetExperienceLevelsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkTypeDto>> GetWorkTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SalaryTypeDto>> GetSalaryTypesAsync(CancellationToken cancellationToken = default);
}
