using Domain.Entities;
using Shared.DTOs.Admin;

namespace ServicesAbstraction;

public interface IAdminHomeService
{
    Task<AdminHomeConfigurationDto> GetAsync(CancellationToken cancellationToken = default);

    Task<AdminHomeSectionDto> UpdateSectionAsync(
        int id, string adminUserId, UpdateHomeSectionRequest request,
        CancellationToken cancellationToken = default);

    Task<AdminHomeSectionDto> UpdateSectionImageAsync(
        int id, string adminUserId, Shared.DTOs.Advertisements.UploadImageModel? image,
        CancellationToken cancellationToken = default);

    Task<AdminHomeSectionDto> DeleteSectionImageAsync(
        int id, string adminUserId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AdminHomeSectionDto>> ReorderAsync(
        ReorderHomeSectionsRequest request, CancellationToken cancellationToken = default);
}

public interface IAdminHomeRepository
{
    Task<IReadOnlyList<HomeSection>> GetSectionsAsync(CancellationToken cancellationToken = default);

    Task<HomeSection?> FindSectionAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeSection>> GetSectionsForUpdateAsync(CancellationToken cancellationToken = default);

    Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
