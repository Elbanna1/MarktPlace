using Shared.DTOs.Advertisements;
using Shared.DTOs.OnlineShopping;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IShoppingElectronicService
{
    Task<ShoppingElectronicDetailsDto> CreateAsync(
        string userId,
        CreateShoppingElectronicRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<ShoppingElectronicDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateShoppingElectronicRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<ShoppingElectronicListItemDto>> GetListAsync(
        ShoppingElectronicFilterParams filter, CancellationToken cancellationToken = default);

    Task<ShoppingElectronicDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetSectionsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetCompatibilitiesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetConditionsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetWarrantiesAsync(
        CancellationToken cancellationToken = default);
}
