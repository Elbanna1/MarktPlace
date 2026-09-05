using Shared.DTOs.Advertisements;
using Shared.DTOs.Craftsmen;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ICraftsmanService
{
    Task<CraftsmanDetailsDto> CreateAsync(
        string userId,
        CreateCraftsmanRequest request,
        IReadOnlyList<UploadImageModel> images);

    Task<CraftsmanDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateCraftsmanRequest request,
        IReadOnlyList<UploadImageModel> newImages);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<CraftsmanListItemDto>> GetListAsync(
        CraftsmanFilterParams filter, CancellationToken cancellationToken = default);

    Task<CraftsmanDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
