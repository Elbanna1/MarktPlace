using Domain.Entities;
using Shared.DTOs.OnlineShopping;

namespace ServicesAbstraction;

public interface IHomemadeFoodRepository
{
    Task<HomemadeFood?> GetByIdAsync(
        Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<HomemadeFood?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<HomemadeFood> Items, int TotalCount)> GetPagedAsync(
        HomemadeFoodFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<HomemadeFoodSectionLookup>> GetSectionsAsync(CancellationToken cancellationToken = default);

    Task<List<HomemadeFoodDeliveryAreaLookup>> GetDeliveryAreasAsync(CancellationToken cancellationToken = default);

    Task AddAsync(HomemadeFood entity);

    void Update(HomemadeFood entity);

    void RemoveImage(HomemadeFoodImage image);

    Task AddImagesAsync(IEnumerable<HomemadeFoodImage> images);

    void RemoveDeliveryAreas(IEnumerable<HomemadeFoodDeliveryAreaSelection> areas);

    Task AddDeliveryAreasAsync(IEnumerable<HomemadeFoodDeliveryAreaSelection> areas);

    Task<int> SaveChangesAsync();
}
