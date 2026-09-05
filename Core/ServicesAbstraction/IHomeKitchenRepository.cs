using Domain.Entities;
using Shared.DTOs.OnlineShopping;

namespace ServicesAbstraction;

public interface IHomeKitchenRepository
{
    Task<HomeKitchen?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<HomeKitchen?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<HomeKitchen> Items, int TotalCount)> GetPagedAsync(
        HomeKitchenFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<HomeKitchenSectionLookup>> GetSectionsAsync(CancellationToken cancellationToken = default);

    Task<List<HomeKitchenMaterialLookup>> GetMaterialsAsync(CancellationToken cancellationToken = default);

    Task<List<HomeKitchenColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(HomeKitchen entity);

    void Update(HomeKitchen entity);

    void RemoveImage(HomeKitchenImage image);

    Task AddImagesAsync(IEnumerable<HomeKitchenImage> images);

    void RemoveColors(IEnumerable<HomeKitchenColorSelection> colors);

    Task AddColorsAsync(IEnumerable<HomeKitchenColorSelection> colors);

    Task<int> SaveChangesAsync();
}
