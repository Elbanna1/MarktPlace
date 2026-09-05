using Shared.DTOs.Advertisements;
using Shared.DTOs.Antiques;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IPaintingService
{
    Task<PaintingDetailsDto> CreateAsync(
        string userId,
        CreatePaintingRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<PaintingDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdatePaintingRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<PaintingListItemDto>> GetListAsync(
        PaintingFilterParams filter, CancellationToken cancellationToken = default);

    Task<PaintingDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetOriginalitiesAsync(CancellationToken cancellationToken = default);
}
