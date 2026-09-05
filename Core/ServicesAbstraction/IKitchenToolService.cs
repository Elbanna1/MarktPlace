using Shared.DTOs.Advertisements;
using Shared.DTOs.HomeFurnishing;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IKitchenToolService
{
    Task<KitchenToolDetailsDto> CreateAsync(
        string userId,
        CreateKitchenToolRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<KitchenToolDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateKitchenToolRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<KitchenToolListItemDto>> GetListAsync(
        KitchenToolFilterParams filter, CancellationToken cancellationToken = default);

    Task<KitchenToolDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<KitchenToolListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<KitchenToolListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<KitchenToolListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        KitchenToolFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default);

    Task<KitchenToolDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderHomeFurnishingImagesRequest request, CancellationToken cancellationToken = default);

    Task<KitchenToolDetailsDto> SetPromotionAsync(
        Guid id, PromoteHomeFurnishingRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetColorsAsync(CancellationToken cancellationToken = default);
}
