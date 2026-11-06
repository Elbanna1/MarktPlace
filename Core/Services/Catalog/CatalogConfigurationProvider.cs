using Microsoft.Extensions.Caching.Memory;
using ServicesAbstraction;
using Shared.Catalog;
using Shared.Constants;

namespace Services.Catalog;

public class CatalogConfigurationProvider : ICatalogConfigurationProvider
{
    public const string CacheKey = "catalog-configuration:snapshot";

    public static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(30);

    private readonly ICatalogConfigurationRepository _configurations;
    private readonly IAdPaymentRepository _payments;
    private readonly ILookupService _lookups;
    private readonly IMemoryCache _cache;

    public CatalogConfigurationProvider(
        ICatalogConfigurationRepository configurations,
        IAdPaymentRepository payments,
        ILookupService lookups,
        IMemoryCache cache)
    {
        _configurations = configurations;
        _payments = payments;
        _lookups = lookups;
        _cache = cache;
    }

    public async Task<CatalogConfigurationSnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(CacheKey, out CatalogConfigurationSnapshot? cached) && cached is not null)
            return cached;

        var rows = await _configurations.GetAllAsync(cancellationToken);
        var settings = await _payments.GetSettingsAsync(asNoTracking: true, cancellationToken);
        var tree = await _lookups.GetCategoriesTreeAsync(cancellationToken);

        var categoryOfSubCategory = new Dictionary<int, int>();

        foreach (var category in tree)
            foreach (var subCategory in category.SubCategories)
                categoryOfSubCategory[subCategory.Id] = category.Id;

        var snapshot = new CatalogConfigurationSnapshot(
            rows.Select(row => new CatalogConfigurationEntry(
                row.Id, row.Kind, row.NodeType, row.NodeId, row.Area, row.IsEnabled, row.IsVisible,
                row.Price, row.Currency, row.ActiveFrom, row.ActiveUntil, row.EnabledAt,
                row.CreatedAt, row.CreatedBy, row.UpdatedAt, row.UpdatedBy)),
            new CatalogPaymentRoot(
                settings?.IsEnabled ?? false,
                settings?.Price ?? 0m,
                settings?.Currency ?? PaymentCatalog.DefaultCurrency,
                settings?.EnabledAt),
            categoryOfSubCategory);

        _cache.Set(CacheKey, snapshot, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheDuration,
            Size = 1
        });

        return snapshot;
    }

    public void Invalidate() => _cache.Remove(CacheKey);
}
