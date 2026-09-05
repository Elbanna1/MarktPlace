using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface IFishRepository
{
    Task<Fish?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Fish?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Fish> Items, int TotalCount)> GetPagedAsync(
        FishFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<FishTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<List<FishPurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<FishAgeLookup>> GetAgesAsync(CancellationToken cancellationToken = default);

    Task<List<FishHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Fish entity);

    void Update(Fish entity);

    void RemoveImage(FishImage image);

    Task AddImagesAsync(IEnumerable<FishImage> images);

    Task<int> SaveChangesAsync();
}
