using Domain.Entities;
using Shared.DTOs.FruitVegetableMerchants;

namespace ServicesAbstraction;

public interface IFruitVegetableMerchantRepository
{
    Task<FruitVegetableMerchant?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<FruitVegetableMerchant?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<FruitVegetableMerchant> Items, int TotalCount)> GetPagedAsync(
        FruitVegetableMerchantFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<MerchantSaleTypeLookup>> GetSaleTypesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(FruitVegetableMerchant merchant);

    void Update(FruitVegetableMerchant merchant);

    void RemoveImage(FruitVegetableMerchantImage image);

    Task AddImagesAsync(IEnumerable<FruitVegetableMerchantImage> images);

    Task<int> SaveChangesAsync();
}
