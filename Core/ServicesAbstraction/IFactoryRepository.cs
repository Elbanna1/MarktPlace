using Domain.Entities;
using Shared.DTOs.Factories;

namespace ServicesAbstraction;

public interface IFactoryRepository
{
    Task<Factory?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Factory?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Factory> Items, int TotalCount)> GetPagedAsync(
        FactoryFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<ProductionSpecialtyLookup>> GetProductionSpecialtiesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Factory factory);

    void Update(Factory factory);

    void RemoveImage(FactoryImage image);

    Task AddImagesAsync(IEnumerable<FactoryImage> images);

    Task<int> SaveChangesAsync();
}
