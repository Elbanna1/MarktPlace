using Shared.DTOs.Advertisements;
using Shared.DTOs.OnlineShopping;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ICosmeticService
{
    Task<CosmeticDetailsDto> CreateAsync(
        string userId,
        CreateCosmeticRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<CosmeticDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateCosmeticRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<CosmeticListItemDto>> GetListAsync(
        CosmeticFilterParams filter, CancellationToken cancellationToken = default);

    Task<CosmeticDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetSectionsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetSuitableForAsync(
        CancellationToken cancellationToken = default);
}
