using Domain.Entities;
using Shared.DTOs.HomeFurnishing;

namespace ServicesAbstraction;

public interface IFurnishingCurtainRepository
{
    Task<FurnishingCurtain?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<FurnishingCurtain?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<FurnishingCurtain> Items, int TotalCount)> GetPagedAsync(
        FurnishingCurtainFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FurnishingCurtain>> GetSimilarAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FurnishingCurtain>> GetRelatedAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<FurnishingCurtain>> GetRecentlyAddedAsync(int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        FurnishingCurtainFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<FurnishingCurtainProductTypeLookup>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<List<FurnishingCurtainSizeLookup>> GetSizesAsync(CancellationToken cancellationToken = default);

    Task<List<FurnishingCurtainMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<FurnishingCurtainColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(FurnishingCurtain entity);

    void Update(FurnishingCurtain entity);

    void RemoveImage(FurnishingCurtainImage image);

    Task AddImagesAsync(IEnumerable<FurnishingCurtainImage> images);

    void RemoveColors(IEnumerable<FurnishingCurtainColorSelection> colors);

    Task AddColorsAsync(IEnumerable<FurnishingCurtainColorSelection> colors);

    Task<int> SaveChangesAsync();
}
