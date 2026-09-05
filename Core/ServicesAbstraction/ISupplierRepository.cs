using Domain.Entities;
using Shared.DTOs.Suppliers;

namespace ServicesAbstraction;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Supplier?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Supplier> Items, int TotalCount)> GetPagedAsync(
        SupplierFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<SupplierSpecializationLookup>> GetSpecializationsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Supplier supplier);

    void Update(Supplier supplier);

    void RemoveImage(SupplierImage image);

    Task AddImagesAsync(IEnumerable<SupplierImage> images);

    Task<int> SaveChangesAsync();
}
