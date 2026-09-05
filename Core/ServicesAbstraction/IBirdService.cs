using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IBirdService
{
    Task<BirdDetailsDto> CreateAsync(
        string userId,
        CreateBirdRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<BirdDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateBirdRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<BirdListItemDto>> GetListAsync(
        BirdFilterParams filter, CancellationToken cancellationToken = default);

    Task<BirdDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetVaccinationsAsync(CancellationToken cancellationToken = default);
}
