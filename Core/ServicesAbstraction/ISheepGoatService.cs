using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ISheepGoatService
{
    Task<SheepGoatDetailsDto> CreateAsync(
        string userId,
        CreateSheepGoatRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<SheepGoatDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateSheepGoatRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<SheepGoatListItemDto>> GetListAsync(
        SheepGoatFilterParams filter, CancellationToken cancellationToken = default);

    Task<SheepGoatDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetVaccinationsAsync(CancellationToken cancellationToken = default);
}
