using Domain.Entities;
using Shared.DTOs.HomeFurnishing;

namespace ServicesAbstraction;

public interface IBathroomSupplyRepository
{
    Task<BathroomSupply?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<BathroomSupply?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<BathroomSupply> Items, int TotalCount)> GetPagedAsync(
        BathroomSupplyFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BathroomSupply>> GetSimilarAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BathroomSupply>> GetRelatedAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BathroomSupply>> GetRecentlyAddedAsync(int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        BathroomSupplyFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<BathroomSupplyProductTypeLookup>> GetProductTypesAsync(CancellationToken cancellationToken = default);

    Task<List<BathroomSupplyMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<BathroomSupplyColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(BathroomSupply entity);

    void Update(BathroomSupply entity);

    void RemoveImage(BathroomSupplyImage image);

    Task AddImagesAsync(IEnumerable<BathroomSupplyImage> images);

    void RemoveColors(IEnumerable<BathroomSupplyColorSelection> colors);

    Task AddColorsAsync(IEnumerable<BathroomSupplyColorSelection> colors);

    Task<int> SaveChangesAsync();
}
