using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IOtherAnimalService
{
    Task<OtherAnimalDetailsDto> CreateAsync(
        string userId,
        CreateOtherAnimalRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<OtherAnimalDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateOtherAnimalRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<OtherAnimalListItemDto>> GetListAsync(
        OtherAnimalFilterParams filter, CancellationToken cancellationToken = default);

    Task<OtherAnimalDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetVaccinationsAsync(CancellationToken cancellationToken = default);
}
