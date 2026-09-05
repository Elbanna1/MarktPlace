using Shared.DTOs.Advertisements;
using Shared.DTOs.Clothing;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IMenClothingService
{
    Task<MenClothingDetailsDto> CreateAsync(
        string userId,
        CreateMenClothingRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<MenClothingDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateMenClothingRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<MenClothingListItemDto>> GetListAsync(
        MenClothingFilterParams filter, CancellationToken cancellationToken = default);

    Task<MenClothingDetailsDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetClothingTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetBrandsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetSizesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ClothingLookupItemDto>> GetSellingMethodsAsync(CancellationToken cancellationToken = default);
}
