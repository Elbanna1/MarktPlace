using Domain.Entities;
using Shared.DTOs.RealEstate;

namespace ServicesAbstraction;

public interface IRealEstateLookupSource
{
    Task<IReadOnlyList<RealEstateLookupItemDto>> GetLookupAsync<TLookup>(
        CancellationToken cancellationToken = default)
        where TLookup : class, IRealEstateLookup;
}

public interface ILandRepository : IRealEstateLookupSource
{
    Task<Land?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Land?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Land> Items, int TotalCount)> GetPagedAsync(
        LandFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Land>> GetSimilarAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Land>> GetRelatedAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Land>> GetRecentlyAddedAsync(int count, CancellationToken cancellationToken = default);

    Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        LandFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Land entity);

    void Update(Land entity);

    void RemoveImage(LandImage image);

    Task AddImagesAsync(IEnumerable<LandImage> images);

    void RemoveUtilities(IEnumerable<LandUtilitySelection> utilities);

    Task AddUtilitiesAsync(IEnumerable<LandUtilitySelection> utilities);

    void RemoveRentInclusions(IEnumerable<LandRentInclusionSelection> inclusions);

    Task AddRentInclusionsAsync(IEnumerable<LandRentInclusionSelection> inclusions);

    Task<int> SaveChangesAsync();
}

public interface IApartmentRepository : IRealEstateLookupSource
{
    Task<Apartment?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Apartment?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Apartment> Items, int TotalCount)> GetPagedAsync(
        ApartmentFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Apartment>> GetSimilarAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Apartment>> GetRelatedAsync(
        Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Apartment>> GetRecentlyAddedAsync(
        int count, CancellationToken cancellationToken = default);

    Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        ApartmentFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Apartment entity);

    void Update(Apartment entity);

    void RemoveImage(ApartmentImage image);

    Task AddImagesAsync(IEnumerable<ApartmentImage> images);

    void RemoveFeatures(IEnumerable<ApartmentFeatureSelection> features);

    Task AddFeaturesAsync(IEnumerable<ApartmentFeatureSelection> features);

    void RemoveRentInclusions(IEnumerable<ApartmentRentInclusionSelection> inclusions);

    Task AddRentInclusionsAsync(IEnumerable<ApartmentRentInclusionSelection> inclusions);

    Task<int> SaveChangesAsync();
}

public interface IShopRepository : IRealEstateLookupSource
{
    Task<Shop?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Shop?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Shop> Items, int TotalCount)> GetPagedAsync(
        ShopFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Shop>> GetSimilarAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Shop>> GetRelatedAsync(Guid id, int count, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Shop>> GetRecentlyAddedAsync(int count, CancellationToken cancellationToken = default);

    Task<RealEstatePriceStatisticsDto> GetPriceStatisticsAsync(
        ShopFilterParams filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RealEstateSuggestionDto>> GetSuggestionsAsync(
        string term, int count, CancellationToken cancellationToken = default);

    Task IncrementViewCountAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Shop entity);

    void Update(Shop entity);

    void RemoveImage(ShopImage image);

    Task AddImagesAsync(IEnumerable<ShopImage> images);

    void RemoveUtilities(IEnumerable<ShopUtilitySelection> utilities);

    Task AddUtilitiesAsync(IEnumerable<ShopUtilitySelection> utilities);

    void RemoveRentInclusions(IEnumerable<ShopRentInclusionSelection> inclusions);

    Task AddRentInclusionsAsync(IEnumerable<ShopRentInclusionSelection> inclusions);

    void RemoveRentSuitableActivities(IEnumerable<ShopRentSuitableActivitySelection> activities);

    Task AddRentSuitableActivitiesAsync(IEnumerable<ShopRentSuitableActivitySelection> activities);

    Task<int> SaveChangesAsync();
}
