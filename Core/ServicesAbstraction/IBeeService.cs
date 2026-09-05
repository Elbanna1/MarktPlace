using Shared.DTOs.Advertisements;
using Shared.DTOs.Animals;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IBeeService
{
    Task<BeeDetailsDto> CreateAsync(
        string userId,
        CreateBeeRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<BeeDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateBeeRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<BeeListItemDto>> GetListAsync(
        BeeFilterParams filter, CancellationToken cancellationToken = default);

    Task<BeeDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AnimalLookupItemDto>> GetProductionsAsync(CancellationToken cancellationToken = default);
}
