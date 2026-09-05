using Shared.DTOs.Advertisements;
using Shared.DTOs.OnlineShopping;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IHomeKitchenService
{
    Task<HomeKitchenDetailsDto> CreateAsync(
        string userId,
        CreateHomeKitchenRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<HomeKitchenDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateHomeKitchenRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<HomeKitchenListItemDto>> GetListAsync(
        HomeKitchenFilterParams filter, CancellationToken cancellationToken = default);

    Task<HomeKitchenDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetSectionsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetMaterialsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetColorsAsync(
        CancellationToken cancellationToken = default);
}
