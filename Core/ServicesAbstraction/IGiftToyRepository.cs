using Domain.Entities;
using Shared.DTOs.OnlineShopping;

namespace ServicesAbstraction;

public interface IGiftToyRepository
{
    Task<GiftToy?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<GiftToy?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<GiftToy> Items, int TotalCount)> GetPagedAsync(
        GiftToyFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<GiftToyTypeLookup>> GetTypesAsync(CancellationToken cancellationToken = default);

    Task<List<GiftToySuitableForLookup>> GetSuitableForAsync(CancellationToken cancellationToken = default);

    Task AddAsync(GiftToy entity);

    void Update(GiftToy entity);

    void RemoveImage(GiftToyImage image);

    Task AddImagesAsync(IEnumerable<GiftToyImage> images);

    Task<int> SaveChangesAsync();
}
