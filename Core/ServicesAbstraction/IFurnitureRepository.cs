using Domain.Entities;
using Shared.DTOs.HomeFurnishing;

namespace ServicesAbstraction;

public interface IFurnitureRepository
{
    Task<Furniture?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Furniture?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Furniture> Items, int TotalCount)> GetPagedAsync(
        FurnitureFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Furniture>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Furniture>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Furniture>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        FurnitureFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<FurnitureTypeLookup>> GetFurnitureTypesAsync(CancellationToken cancellationToken = default);

    Task<List<FurnitureMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<FurnitureColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<List<FurnitureConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Furniture entity);

    void Update(Furniture entity);

    void RemoveImage(FurnitureImage image);

    Task AddImagesAsync(IEnumerable<FurnitureImage> images);

    void RemoveColors(IEnumerable<FurnitureColorSelection> colors);

    Task AddColorsAsync(IEnumerable<FurnitureColorSelection> colors);

    Task<int> SaveChangesAsync();
}
