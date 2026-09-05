using Domain.Entities;
using Shared.DTOs.OnlineShopping;

namespace ServicesAbstraction;

public interface IAccessoryRepository
{
    Task<Accessory?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Accessory?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Accessory> Items, int TotalCount)> GetPagedAsync(
        AccessoryFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<AccessoryTypeLookup>> GetAccessoryTypesAsync(CancellationToken cancellationToken = default);

    Task<List<AccessoryCategoryLookup>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<List<AccessoryMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<AccessoryColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Accessory entity);

    void Update(Accessory entity);

    void RemoveImage(AccessoryImage image);

    Task AddImagesAsync(IEnumerable<AccessoryImage> images);

    void RemoveColors(IEnumerable<AccessoryColorSelection> colors);

    Task AddColorsAsync(IEnumerable<AccessoryColorSelection> colors);

    Task<int> SaveChangesAsync();
}
