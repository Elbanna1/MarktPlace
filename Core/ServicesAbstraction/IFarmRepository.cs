using Domain.Entities;
using Shared.DTOs.Farms;

namespace ServicesAbstraction;

public interface IFarmRepository
{
    Task<Farm?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Farm?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Farm> Items, int TotalCount)> GetPagedAsync(
        FarmFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<FarmTypeLookup>> GetFarmTypesAsync(CancellationToken cancellationToken = default);

    Task<List<FarmingMethodLookup>> GetFarmingMethodsAsync(CancellationToken cancellationToken = default);

    Task<List<AvailabilitySeasonLookup>> GetAvailabilitySeasonsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Farm farm);

    void Update(Farm farm);

    void RemoveImage(FarmImage image);

    Task AddImagesAsync(IEnumerable<FarmImage> images);

    Task<int> SaveChangesAsync();
}
