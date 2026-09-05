using Shared.DTOs.Advertisements;
using Shared.DTOs.HomeFurnishing;
using Shared.Responses;

namespace ServicesAbstraction;

public interface IBathroomSupplyService
{
    Task<BathroomSupplyDetailsDto> CreateAsync(
        string userId,
        CreateBathroomSupplyRequest request,
        IReadOnlyList<UploadImageModel> images,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task<BathroomSupplyDetailsDto> UpdateAsync(
        string userId,
        bool isAdmin,
        Guid id,
        UpdateBathroomSupplyRequest request,
        IReadOnlyList<UploadImageModel> newImages,
        UploadImageModel? video,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId, Guid id, bool isAdmin);

    Task<PaginatedResult<BathroomSupplyListItemDto>> GetListAsync(
        BathroomSupplyFilterParams filter, CancellationToken cancellationToken = default);

    Task<BathroomSupplyDetailsDto> GetByIdAsync(
        Guid id, bool countView = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BathroomSupplyListItemDto>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BathroomSupplyListItemDto>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BathroomSupplyListItemDto>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        BathroomSupplyFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string? term, int count, CancellationToken cancellationToken = default);

    Task<BathroomSupplyDetailsDto> ReorderImagesAsync(
        string userId, bool isAdmin, Guid id,
        ReorderHomeFurnishingImagesRequest request, CancellationToken cancellationToken = default);

    Task<BathroomSupplyDetailsDto> SetPromotionAsync(
        Guid id, PromoteHomeFurnishingRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingLookupItemDto>> GetColorsAsync(CancellationToken cancellationToken = default);
}
