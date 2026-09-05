using Domain.Entities;
using Shared.DTOs.HomeFurnishing;

namespace ServicesAbstraction;

public interface ILightingDecorRepository
{
    Task<LightingDecor?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<LightingDecor?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<LightingDecor> Items, int TotalCount)> GetPagedAsync(
        LightingDecorFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LightingDecor>> GetSimilarAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LightingDecor>> GetRelatedAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LightingDecor>> GetRecentlyAddedAsync(int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        LightingDecorFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<LightingDecorProductTypeLookup>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<List<LightingDecorMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<LightingDecorColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<List<LightingDecorLightTypeLookup>> GetLightTypesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(LightingDecor entity);

    void Update(LightingDecor entity);

    void RemoveImage(LightingDecorImage image);

    Task AddImagesAsync(IEnumerable<LightingDecorImage> images);

    void RemoveColors(IEnumerable<LightingDecorColorSelection> colors);

    Task AddColorsAsync(IEnumerable<LightingDecorColorSelection> colors);

    Task<int> SaveChangesAsync();
}
