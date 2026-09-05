using Shared.DTOs.Advertisements;
using Shared.DTOs.Farms;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IFarmService
{
    Task<FarmDetailsDto> CreateAsync(
        string userId, CreateFarmRequest request, IReadOnlyList<UploadImageModel> images);

    Task<FarmDetailsDto> UpdateAsync(
        string userId, bool isAdmin, Guid id, UpdateFarmRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<FarmListItemDto>> GetListAsync(
        FarmFilterParams filter, CancellationToken cancellationToken = default);

    Task<FarmDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FarmTypeOptionDto>> GetFarmTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FarmingMethodOptionDto>> GetFarmingMethodsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AvailabilitySeasonOptionDto>> GetAvailabilitySeasonsAsync(
        CancellationToken cancellationToken = default);
}
