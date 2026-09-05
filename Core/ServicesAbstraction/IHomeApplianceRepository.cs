using Domain.Entities;
using Shared.DTOs.HomeFurnishing;

namespace ServicesAbstraction;

public interface IHomeApplianceRepository
{
    Task<HomeAppliance?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<HomeAppliance?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<HomeAppliance> Items, int TotalCount)> GetPagedAsync(
        HomeApplianceFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeAppliance>> GetSimilarAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeAppliance>> GetRelatedAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeAppliance>> GetRecentlyAddedAsync(int count, CancellationToken cancellationToken = default);

    Task<HomeFurnishingPriceStatisticsDto> GetPriceStatisticsAsync(
        HomeApplianceFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<HomeFurnishingSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<HomeApplianceDeviceTypeLookup>> GetDeviceTypesAsync(CancellationToken cancellationToken = default);

    Task<List<HomeApplianceBrandLookup>> GetBrandsAsync(CancellationToken cancellationToken = default);

    Task<List<HomeApplianceConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<List<HomeApplianceWarrantyLookup>> GetWarrantiesAsync(CancellationToken cancellationToken = default);

    Task<List<HomeApplianceColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(HomeAppliance entity);

    void Update(HomeAppliance entity);

    void RemoveImage(HomeApplianceImage image);

    Task AddImagesAsync(IEnumerable<HomeApplianceImage> images);

    void RemoveColors(IEnumerable<HomeApplianceColorSelection> colors);

    Task AddColorsAsync(IEnumerable<HomeApplianceColorSelection> colors);

    Task<int> SaveChangesAsync();
}
