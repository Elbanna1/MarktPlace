using Domain.Entities;
using Shared.DTOs.Craftsmen;

namespace ServicesAbstraction;

public interface ICraftsmanRepository
{
    Task<Craftsman?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Craftsman?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Craftsman> Items, int TotalCount)> GetPagedAsync(
        CraftsmanFilterParams filter, CancellationToken cancellationToken = default);

    Task AddAsync(Craftsman craftsman);

    void Update(Craftsman craftsman);

    void RemoveImage(CraftsmanImage image);

    Task AddImagesAsync(IEnumerable<CraftsmanImage> images);

    Task<int> SaveChangesAsync();
}
