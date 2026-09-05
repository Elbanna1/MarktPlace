using Domain.Entities;
using Shared.DTOs.OnlineShopping;

namespace ServicesAbstraction;

public interface IShoppingElectronicRepository
{
    Task<ShoppingElectronic?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<ShoppingElectronic?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ShoppingElectronic> Items, int TotalCount)> GetPagedAsync(
        ShoppingElectronicFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<ShoppingElectronicSectionLookup>> GetSectionsAsync(CancellationToken cancellationToken = default);

    Task<List<ShoppingElectronicCompatibilityLookup>> GetCompatibilitiesAsync(
        CancellationToken cancellationToken = default);

    Task<List<ShoppingElectronicConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<List<ShoppingElectronicWarrantyLookup>> GetWarrantiesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(ShoppingElectronic entity);

    void Update(ShoppingElectronic entity);

    void RemoveImage(ShoppingElectronicImage image);

    Task AddImagesAsync(IEnumerable<ShoppingElectronicImage> images);

    Task<int> SaveChangesAsync();
}
