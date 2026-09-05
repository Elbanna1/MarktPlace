using Shared.DTOs.Advertisements;
using Shared.DTOs.HomeFurnishing;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IFurnitureService
{
    Task<FurnitureDetailsDto> CreateAsync(
        string userId,
        CreateFurnitureRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<FurnitureDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateFurnitureRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<FurnitureListItemDto>> GetListAsync(
        FurnitureFilterParams filter, CancellationToken cancellationToken = default);

    Task<FurnitureDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FurnitureListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FurnitureListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FurnitureListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        FurnitureFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default);

    Task<FurnitureDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderHomeFurnishingImagesRequest request, CancellationToken cancellationToken = default);

    Task<FurnitureDetailsDto> SetPromotionAsync(
        Guid id, PromoteHomeFurnishingRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetFurnitureTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetConditionsAsync(CancellationToken cancellationToken = default);
}
