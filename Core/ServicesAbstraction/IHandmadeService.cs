using Shared.DTOs.Advertisements;
using Shared.DTOs.Antiques;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IHandmadeService
{
    Task<HandmadeDetailsDto> CreateAsync(
        string userId,
        CreateHandmadeRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<HandmadeDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateHandmadeRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<HandmadeListItemDto>> GetListAsync(
        HandmadeFilterParams filter, CancellationToken cancellationToken = default);

    Task<HandmadeDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AntiqueLookupItemDto>> GetColorsAsync(CancellationToken cancellationToken = default);
}
