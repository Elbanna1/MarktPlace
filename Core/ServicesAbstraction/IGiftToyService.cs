using Shared.DTOs.Advertisements;
using Shared.DTOs.OnlineShopping;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IGiftToyService
{
    Task<GiftToyDetailsDto> CreateAsync(
        string userId,
        CreateGiftToyRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<GiftToyDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateGiftToyRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<GiftToyListItemDto>> GetListAsync(
        GiftToyFilterParams filter, CancellationToken cancellationToken = default);

    Task<GiftToyDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetTypesAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OnlineShoppingLookupItemDto>> GetSuitableForAsync(
        CancellationToken cancellationToken = default);
}
