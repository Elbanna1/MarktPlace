using Domain.Entities;
using Shared.DTOs.JobRequests;

namespace ServicesAbstraction;

public interface IJobRequestRepository
{
    Task<JobRequest?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<JobRequest?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<JobRequest> Items, int TotalCount)> GetPagedAsync(
        JobRequestFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<JobFieldLookup>> GetJobFieldsAsync(CancellationToken cancellationToken = default);

    Task<List<JobExperienceLevelLookup>> GetExperienceLevelsAsync(CancellationToken cancellationToken = default);

    Task<List<EducationLevelLookup>> GetEducationLevelsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(JobRequest jobRequest);

    void Update(JobRequest jobRequest);

    Task<int> SaveChangesAsync();
}
