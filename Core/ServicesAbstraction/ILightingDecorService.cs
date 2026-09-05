using Shared.DTOs.Advertisements;
using Shared.DTOs.HomeFurnishing;
using Shared.Responses;

namespace ServicesAbstraction;

public interface ILightingDecorService
{
    Task<LightingDecorDetailsDto> CreateAsync(
        string userId,
        CreateLightingDecorRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<LightingDecorDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateLightingDecorRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<LightingDecorListItemDto>> GetListAsync(
        LightingDecorFilterParams filter, CancellationToken cancellationToken = default);

    Task<LightingDecorDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LightingDecorListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LightingDecorListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LightingDecorListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        LightingDecorFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default);

    Task<LightingDecorDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderHomeFurnishingImagesRequest request, CancellationToken cancellationToken = default);

    Task<LightingDecorDetailsDto> SetPromotionAsync(
        Guid id, PromoteHomeFurnishingRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetLightTypesAsync(CancellationToken cancellationToken = default);
}
