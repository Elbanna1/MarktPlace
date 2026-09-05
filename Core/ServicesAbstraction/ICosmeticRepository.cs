using Domain.Entities;
using Shared.DTOs.OnlineShopping;

namespace ServicesAbstraction;

public interface ICosmeticRepository
{
    Task<Cosmetic?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Cosmetic?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Cosmetic> Items, int TotalCount)> GetPagedAsync(
        CosmeticFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<CosmeticSectionLookup>> GetSectionsAsync(CancellationToken cancellationToken = default);

    Task<List<CosmeticSuitableForLookup>> GetSuitableForAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Cosmetic entity);

    void Update(Cosmetic entity);

    void RemoveImage(CosmeticImage image);

    Task AddImagesAsync(IEnumerable<CosmeticImage> images);

    Task<int> SaveChangesAsync();
}
