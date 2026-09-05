using Domain.Entities;
using Shared.DTOs.Clothing;

namespace ServicesAbstraction;

public interface IMenClothingRepository
{
    Task<MenClothing?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<MenClothing?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<MenClothing> Items, int TotalCount)> GetPagedAsync(
        MenClothingFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<MenClothingTypeLookup>> GetClothingTypesAsync(CancellationToken cancellationToken = default);

    Task<List<MenClothingBrandLookup>> GetBrandsAsync(CancellationToken cancellationToken = default);

    Task<List<MenClothingSizeLookup>> GetSizesAsync(CancellationToken cancellationToken = default);

    Task<List<MenClothingColorLookup>> GetColorsAsync(CancellationToken cancellationToken = default);

    Task<List<MenClothingConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task<List<MenClothingSellingMethodLookup>> GetSellingMethodsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(MenClothing entity);

    void Update(MenClothing entity);

    void RemoveImage(MenClothingImage image);

    Task AddImagesAsync(IEnumerable<MenClothingImage> images);

    void RemoveSizes(IEnumerable<MenClothingSizeSelection> sizes);

    Task AddSizesAsync(IEnumerable<MenClothingSizeSelection> sizes);

    void RemoveColors(IEnumerable<MenClothingColorSelection> colors);

    Task AddColorsAsync(IEnumerable<MenClothingColorSelection> colors);

    Task<int> SaveChangesAsync();
}
