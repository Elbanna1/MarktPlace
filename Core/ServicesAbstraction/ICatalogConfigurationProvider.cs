using Shared.Catalog;

namespace ServicesAbstraction;

public interface ICatalogConfigurationProvider
{
    Task<CatalogConfigurationSnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default);

    void Invalidate();
}
