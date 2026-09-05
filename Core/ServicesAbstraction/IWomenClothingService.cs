using Shared.DTOs.Advertisements;
using Shared.DTOs.Clothing;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IWomenClothingService
{
    Task<WomenClothingDetailsDto> CreateAsync(
        string userId,
        CreateWomenClothingRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<WomenClothingDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateWomenClothingRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<WomenClothingListItemDto>> GetListAsync(
        WomenClothingFilterParams filter, CancellationToken cancellationToken = default);

    Task<WomenClothingDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetClothingTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetBrandsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetSizesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetSellingMethodsAsync(CancellationToken cancellationToken = default);
}
