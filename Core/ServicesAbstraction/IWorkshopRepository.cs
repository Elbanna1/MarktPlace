using Domain.Entities;
using Shared.DTOs.Workshops;

namespace ServicesAbstraction;

public interface IWorkshopRepository
{
    Task<Workshop?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Workshop?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Workshop> Items, int TotalCount)> GetPagedAsync(
        WorkshopFilterParams filter, CancellationToken cancellationToken = default);

    Task AddAsync(Workshop workshop);

    void Update(Workshop workshop);

    void RemoveImage(WorkshopImage image);

    Task AddImagesAsync(IEnumerable<WorkshopImage> images);

    Task<int> SaveChangesAsync();
}
