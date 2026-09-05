using Domain.Entities;
using Shared.DTOs.Clothing;

namespace ServicesAbstraction;

public interface IKidsClothingRepository
{
    Task<KidsClothing?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<KidsClothing?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<KidsClothing> Items, int TotalCount)> GetPagedAsync(
        KidsClothingFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<KidsClothingTypeLookup>> GetClothingTypesAsync(CancellationToken cancellationToken = default);

    Task<List<KidsClothingBrandLookup>> GetBrandsAsync(CancellationToken cancellationToken = default);

    Task<List<KidsClothingSizeLookup>> GetSizesAsync(CancellationToken cancellationToken = default);

    Task<List<KidsClothingColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<List<KidsClothingConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<List<KidsClothingSellingMethodLookup>> GetSellingMethodsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(KidsClothing entity);

    void Update(KidsClothing entity);

    void RemoveImage(KidsClothingImage image);

    Task AddImagesAsync(IEnumerable<KidsClothingImage> images);

    void RemoveSizes(IEnumerable<KidsClothingSizeSelection> sizes);

    Task AddSizesAsync(IEnumerable<KidsClothingSizeSelection> sizes);

    void RemoveColors(IEnumerable<KidsClothingColorSelection> colors);

    Task AddColorsAsync(IEnumerable<KidsClothingColorSelection> colors);

    Task<int> SaveChangesAsync();
}
