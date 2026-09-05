using Domain.Entities;
using Shared.DTOs.Admin;

namespace ServicesAbstraction;

public interface IAdminLocationService
{
    Task<IReadOnlyList<AdminGovernorateDto>> GetGovernoratesAsync(CancellationToken cancellationToken = default);

    Task<AdminGovernorateDto> CreateGovernorateAsync(
        SaveLocationRequest request, CancellationToken cancellationToken = default);

    Task<AdminGovernorateDto> UpdateGovernorateAsync(
        int id, SaveLocationRequest request, CancellationToken cancellationToken = default);

    Task DeleteGovernorateAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminCenterDto>> GetCentersAsync(
        int governorateId, CancellationToken cancellationToken = default);

    Task<AdminCenterDto> CreateCenterAsync(
        int governorateId, SaveLocationRequest request, CancellationToken cancellationToken = default);

    Task<AdminCenterDto> UpdateCenterAsync(
        int id, SaveLocationRequest request, CancellationToken cancellationToken = default);

    Task DeleteCenterAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminProjectDto>> GetProjectsAsync(
        int? centerId = null, CancellationToken cancellationToken = default);

    Task<AdminProjectDto> CreateProjectAsync(
        CreateProjectRequest request, CancellationToken cancellationToken = default);

    Task<AdminProjectDto> UpdateProjectAsync(
        int id, UpdateProjectRequest request, CancellationToken cancellationToken = default);

    Task DeleteProjectAsync(int id, CancellationToken cancellationToken = default);
}

public interface IAdminLocationRepository
{
    Task<IReadOnlyList<Governorate>> GetGovernoratesAsync(CancellationToken cancellationToken = default);

    Task<Governorate?> FindGovernorateAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Center>> GetCentersAsync(
        int? governorateId = null, CancellationToken cancellationToken = default);

    Task<Center?> FindCenterAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Project>> GetProjectsAsync(
        int? centerId = null, CancellationToken cancellationToken = default);

    Task<Project?> FindProjectAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> GovernorateNameExistsAsync(
        string name, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> CenterNameExistsAsync(
        int governorateId, string name, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<bool> ProjectNameExistsAsync(
        int centerId, string name, int? excludeId = null, CancellationToken cancellationToken = default);

    Task<int> CountUsersInGovernorateAsync(string name, CancellationToken cancellationToken = default);

    Task<int> CountUsersInCenterAsync(string name, CancellationToken cancellationToken = default);

    Task<int> NextGovernorateIdAsync(CancellationToken cancellationToken = default);

    Task<int> NextCenterIdAsync(CancellationToken cancellationToken = default);

    Task<int> NextProjectIdAsync(CancellationToken cancellationToken = default);

    void AddGovernorate(Governorate governorate);

    void RemoveGovernorate(Governorate governorate);

    void AddCenter(Center center);

    void RemoveCenter(Center center);

    void AddProject(Project project);

    void RemoveProject(Project project);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
