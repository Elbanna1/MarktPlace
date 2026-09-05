using Domain.Entities;
using Shared.DTOs.Advertisements;

namespace ServicesAbstraction;

public interface IAdvertisementRepository
{
    Task<Advertisement?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<Advertisement?> GetDetailsAsync(
        Guid id, bool includeUnmoderated = false, CancellationToken cancellationToken = default);

    Task<Advertisement?> GetOwnedAsync(Guid id, string ownerId, CancellationToken cancellationToken = default);

    Task<AdvertisementOwnership?> GetOwnershipAsync(Guid id, CancellationToken cancellationToken = default);

    Task IncrementViewsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<OwnerAdvertisementCount>> GetOwnerStatusCountsAsync(
        string ownerId, DateTime utcNow, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<AdvertisementCardRow> Items, int TotalCount)> GetPagedAsync(
        AdvertisementFilterParams filter,
        string? ownerId = null,
        bool includeNonActive = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Advertisement>> GetAdvertisementsExpiringBetweenAsync(DateTime fromUtc, DateTime toUtc);

    Task<IReadOnlyList<Advertisement>> GetAdvertisementsToExpireAsync(DateTime utcNow);

    Task<IReadOnlyList<Advertisement>> GetAdvertisementsToDeleteAsync(DateTime utcNow);

    Task<AdvertisementImage?> GetImageAsync(Guid imageId);

    Task<List<Feature>> GetFeaturesByIdsAsync(IEnumerable<int> ids);

    Task<bool> SubCategoryExistsAsync(int subCategoryId);

    Task<int?> GetSubCategoryCategoryIdAsync(int subCategoryId);

    Task AddAsync(Advertisement advertisement);

    void Update(Advertisement advertisement);

    void Remove(Advertisement advertisement);

    void RemoveImage(AdvertisementImage image);

    Task AddImagesAsync(IEnumerable<AdvertisementImage> images);

    Task AddViewAsync(AdvertisementView view);

    Task<int> SaveChangesAsync();
}
