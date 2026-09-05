using Domain.Entities;
using Shared.DTOs.HomeFurnishing;

namespace ServicesAbstraction;

public interface IPlantOrnamentRepository
{
    Task<PlantOrnament?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<PlantOrnament?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<PlantOrnament> Items, int TotalCount)> GetPagedAsync(
        PlantOrnamentFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlantOrnament>> GetSimilarAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlantOrnament>> GetRelatedAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlantOrnament>> GetRecentlyAddedAsync(int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        PlantOrnamentFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<PlantOrnamentProductTypeLookup>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<List<PlantOrnamentSuitableForLookup>> GetSuitableForsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(PlantOrnament entity);

    void Update(PlantOrnament entity);

    void RemoveImage(PlantOrnamentImage image);

    Task AddImagesAsync(IEnumerable<PlantOrnamentImage> images);

    Task<int> SaveChangesAsync();
}
