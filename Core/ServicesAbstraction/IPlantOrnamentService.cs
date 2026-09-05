using Shared.DTOs.Advertisements;
using Shared.DTOs.HomeFurnishing;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IPlantOrnamentService
{
    Task<PlantOrnamentDetailsDto> CreateAsync(
        string userId,
        CreatePlantOrnamentRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<PlantOrnamentDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdatePlantOrnamentRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<PlantOrnamentListItemDto>> GetListAsync(
        PlantOrnamentFilterParams filter, CancellationToken cancellationToken = default);

    Task<PlantOrnamentDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlantOrnamentListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlantOrnamentListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlantOrnamentListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        PlantOrnamentFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default);

    Task<PlantOrnamentDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderHomeFurnishingImagesRequest request, CancellationToken cancellationToken = default);

    Task<PlantOrnamentDetailsDto> SetPromotionAsync(
        Guid id, PromoteHomeFurnishingRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetSuitableForsAsync(CancellationToken cancellationToken = default);
}
