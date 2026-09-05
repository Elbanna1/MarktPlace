using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IHorseService
{
    Task<HorseDetailsDto> CreateAsync(
        string userId,
        CreateHorseRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<HorseDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateHorseRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<HorseListItemDto>> GetListAsync(
        HorseFilterParams filter, CancellationToken cancellationToken = default);

    Task<HorseDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetTrainingLevelsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetVaccinationsAsync(CancellationToken cancellationToken = default);
}
