using Shared.DTOs.Advertisements;
using Shared.DTOs.OnlineShopping;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IAccessoryService
{
    Task<AccessoryDetailsDto> CreateAsync(
        string userId,
        CreateAccessoryRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? logo,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<AccessoryDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateAccessoryRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? logo,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<AccessoryListItemDto>> GetListAsync(
        AccessoryFilterParams filter, CancellationToken cancellationToken = default);

    Task<AccessoryDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetAccessoryTypesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetCategoriesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetMaterialsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetColorsAsync(
        CancellationToken cancellationToken = default);
}
