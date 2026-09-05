using Domain.Entities;
using Shared.DTOs.Clothing;

namespace ServicesAbstraction;

public interface IWomenClothingRepository
{
    Task<WomenClothing?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<WomenClothing?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<WomenClothing> Items, int TotalCount)> GetPagedAsync(
        WomenClothingFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<WomenClothingTypeLookup>> GetClothingTypesAsync(CancellationToken cancellationToken = default);

    Task<List<WomenClothingBrandLookup>> GetBrandsAsync(CancellationToken cancellationToken = default);

    Task<List<WomenClothingSizeLookup>> GetSizesAsync(CancellationToken cancellationToken = default);

    Task<List<WomenClothingColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<List<WomenClothingConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<List<WomenClothingSellingMethodLookup>> GetSellingMethodsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(WomenClothing entity);

    void Update(WomenClothing entity);

    void RemoveImage(WomenClothingImage image);

    Task AddImagesAsync(IEnumerable<WomenClothingImage> images);

    void RemoveSizes(IEnumerable<WomenClothingSizeSelection> sizes);

    Task AddSizesAsync(IEnumerable<WomenClothingSizeSelection> sizes);

    void RemoveColors(IEnumerable<WomenClothingColorSelection> colors);

    Task AddColorsAsync(IEnumerable<WomenClothingColorSelection> colors);

    Task<int> SaveChangesAsync();
}
