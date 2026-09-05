using Shared.DTOs.Advertisements;
using Shared.DTOs.Workshops;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IWorkshopService
{
    Task<WorkshopDetailsDto> CreateAsync(
        string userId,
        CreateWorkshopRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<WorkshopDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateWorkshopRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<WorkshopListItemDto>> GetListAsync(
        WorkshopFilterParams filter, CancellationToken cancellationToken = default);

    Task<WorkshopDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
