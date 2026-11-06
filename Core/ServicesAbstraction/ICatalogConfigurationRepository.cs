using Domain.Entities;
using Shared.Enums;

namespace ServicesAbstraction;

public interface ICatalogConfigurationRepository
{
    Task<IReadOnlyList<CatalogConfiguration>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<CatalogConfiguration>> GetTrackedAsync(
        CatalogConfigurationKind kind, CancellationToken cancellationToken = default);

    void Add(CatalogConfiguration configuration);

    void Remove(CatalogConfiguration configuration);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
