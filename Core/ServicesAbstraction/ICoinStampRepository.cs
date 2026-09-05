using Domain.Entities;
using Shared.DTOs.Antiques;

namespace ServicesAbstraction;

public interface ICoinStampRepository
{
    Task<CoinStamp?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<CoinStamp?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<CoinStamp> Items, int TotalCount)> GetPagedAsync(
        CoinStampFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<CoinStampItemTypeLookup>> GetItemTypesAsync(CancellationToken cancellationToken = default);

    Task<List<CoinStampMetalLookup>> GetMetalsAsync(CancellationToken cancellationToken = default);

    Task<List<CoinStampConditionLookup>> GetConditionsAsync(CancellationToken cancellationToken = default);

    Task AddAsync(CoinStamp entity);

    void Update(CoinStamp entity);

    void RemoveImage(CoinStampImage image);

    Task AddImagesAsync(IEnumerable<CoinStampImage> images);

    void RemoveVideo(CoinStampVideo video);

    Task AddVideoAsync(CoinStampVideo video);

    Task<int> SaveChangesAsync();
}
