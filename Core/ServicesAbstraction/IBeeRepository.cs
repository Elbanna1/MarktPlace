using Domain.Entities;
using Shared.DTOs.Animals;

namespace ServicesAbstraction;

public interface IBeeRepository
{
    Task<Bee?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Bee?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Bee> Items, int TotalCount)> GetPagedAsync(
        BeeFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<BeeTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<List<BeePurposeLookup>> GetPurposesAsync(CancellationToken cancellationToken = default);

    Task<List<BeeHealthStatusLookup>> GetHealthStatusesAsync(CancellationToken cancellationToken = default);

    Task<List<BeeProductionLookup>> GetProductionsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Bee entity);

    void Update(Bee entity);

    void RemoveImage(BeeImage image);

    Task AddImagesAsync(IEnumerable<BeeImage> images);

    Task<int> SaveChangesAsync();
}
