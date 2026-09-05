using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IPetService
{
    Task<PetDetailsDto> CreateAsync(
        string userId,
        CreatePetRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<PetDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdatePetRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<PetListItemDto>> GetListAsync(
        PetFilterParams filter, CancellationToken cancellationToken = default);

    Task<PetDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetTrainingLevelsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetVaccinationsAsync(CancellationToken cancellationToken = default);
}
