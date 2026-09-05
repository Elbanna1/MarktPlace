using Domain.Entities;
using Shared.DTOs.WholesaleTraders;

namespace ServicesAbstraction;

public interface IWholesaleTraderRepository
{
    Task<WholesaleTrader?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default, bool includeUnmoderated = false);

    Task<WholesaleTrader?> GetOwnedAsync(Guid id, string userId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<WholesaleTrader> Items, int TotalCount)> GetPagedAsync(
        WholesaleTraderFilterParams filter, CancellationToken cancellationToken = default);

    Task<List<WholesaleTradeTypeLookup>> GetTradeTypesAsync(CancellationToken cancellationToken = default);

    Task AddAsync(WholesaleTrader trader);

    void Update(WholesaleTrader trader);

    void RemoveImage(WholesaleTraderImage image);

    Task AddImagesAsync(IEnumerable<WholesaleTraderImage> images);

    Task<int> SaveChangesAsync();
}
