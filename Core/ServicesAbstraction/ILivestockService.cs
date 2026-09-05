using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ILivestockService
{
    Task<LivestockDetailsDto> CreateAsync(
        string userId,
        CreateLivestockRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<LivestockDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateLivestockRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<LivestockListItemDto>> GetListAsync(
        LivestockFilterParams filter, CancellationToken cancellationToken = default);

    Task<LivestockDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetBreedsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetGendersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetVaccinationsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetProductionsAsync(CancellationToken cancellationToken = default);
}
