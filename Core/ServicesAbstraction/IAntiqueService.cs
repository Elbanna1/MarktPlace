using Shared.DTOs.Advertisements;
using Shared.DTOs.Antiques;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAntiqueService
{
    Task<AntiqueDetailsDto> CreateAsync(
        string userId,
        CreateAntiqueRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<AntiqueDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateAntiqueRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<AntiqueListItemDto>> GetListAsync(
        AntiqueFilterParams filter, CancellationToken cancellationToken = default);

    Task<AntiqueDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetWorkingStatusesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetOriginalitiesAsync(CancellationToken cancellationToken = default);
}
