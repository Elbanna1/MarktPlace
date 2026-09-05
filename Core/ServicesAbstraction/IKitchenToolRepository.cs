using Domain.Entities;
using Shared.DTOs.HomeFurnishing;

namespace ServicesAbstraction;

public interface IKitchenToolRepository
{
    Task<KitchenTool?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<KitchenTool?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<KitchenTool> Items, int TotalCount)> GetPagedAsync(
        KitchenToolFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<KitchenTool>> GetSimilarAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<KitchenTool>> GetRelatedAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<KitchenTool>> GetRecentlyAddedAsync(int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        KitchenToolFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<KitchenToolProductTypeLookup>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<List<KitchenToolMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<KitchenToolColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(KitchenTool entity);

    void Update(KitchenTool entity);

    void RemoveImage(KitchenToolImage image);

    Task AddImagesAsync(IEnumerable<KitchenToolImage> images);

    void RemoveColors(IEnumerable<KitchenToolColorSelection> colors);

    Task AddColorsAsync(IEnumerable<KitchenToolColorSelection> colors);

    Task<int> SaveChangesAsync();
}
