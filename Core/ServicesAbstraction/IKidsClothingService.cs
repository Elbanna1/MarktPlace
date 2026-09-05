using Shared.DTOs.Advertisements;
using Shared.DTOs.Clothing;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IKidsClothingService
{
    Task<KidsClothingDetailsDto> CreateAsync(
        string userId,
        CreateKidsClothingRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<KidsClothingDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateKidsClothingRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<KidsClothingListItemDto>> GetListAsync(
        KidsClothingFilterParams filter, CancellationToken cancellationToken = default);

    Task<KidsClothingDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetClothingTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetBrandsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetSizesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetSellingMethodsAsync(CancellationToken cancellationToken = default);
}
