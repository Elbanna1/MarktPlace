using Domain.Entities;
using Shared.DTOs.JobOpportunities;

namespace ServicesAbstraction;

public interface IJobOpportunityRepository
{
    Task<JobOpportunity?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<JobOpportunity?> GetOwnedAsync(
        Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<JobOpportunity> Items, int TotalCount)> GetPagedAsync(
        JobOpportunityFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<JobFieldLookup>> GetJobFieldsAsync(CancellationToken cancellationToken = default);

    Task<List<JobExperienceLevelLookup>> GetExperienceLevelsAsync(CancellationToken cancellationToken = default);

    Task<List<WorkTypeLookup>> GetWorkTypesAsync(CancellationToken cancellationToken = default);

    Task<List<SalaryTypeLookup>> GetSalaryTypesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(JobOpportunity jobOpportunity);

    void Update(JobOpportunity jobOpportunity);

    void RemoveImage(JobOpportunityImage image);

    Task AddImagesAsync(IEnumerable<JobOpportunityImage> images);

    Task<int> SaveChangesAsync();
}
