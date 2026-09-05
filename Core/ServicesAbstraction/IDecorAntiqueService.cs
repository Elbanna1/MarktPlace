using Shared.DTOs.Advertisements;
using Shared.DTOs.Antiques;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IDecorAntiqueService
{
    Task<DecorAntiqueDetailsDto> CreateAsync(
        string userId,
        CreateDecorAntiqueRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<DecorAntiqueDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateDecorAntiqueRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<DecorAntiqueListItemDto>> GetListAsync(
        DecorAntiqueFilterParams filter, CancellationToken cancellationToken = default);

    Task<DecorAntiqueDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetItemTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetOriginalitiesAsync(CancellationToken cancellationToken = default);
}
