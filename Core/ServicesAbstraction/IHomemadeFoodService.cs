using Shared.DTOs.Advertisements;
using Shared.DTOs.OnlineShopping;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IHomemadeFoodService
{
    Task<HomemadeFoodDetailsDto> CreateAsync(
        string userId,
        CreateHomemadeFoodRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<HomemadeFoodDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateHomemadeFoodRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<HomemadeFoodListItemDto>> GetListAsync(
        HomemadeFoodFilterParams filter, CancellationToken cancellationToken = default);

    Task<HomemadeFoodDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetSectionsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetDeliveryAreasAsync(
        CancellationToken cancellationToken = default);
}
