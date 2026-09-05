using Domain.Entities;

namespace ServicesAbstraction;

public interface IBannerRepository
{
    Task<Banner?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Banner>> GetActiveAsync(
        DateTime utcNow, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Banner>> GetAllAsync(
        bool? isActive = null, CancellationToken cancellationToken = default);

    void Add(Banner banner);

    void Update(Banner banner);

    void Remove(Banner banner);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
