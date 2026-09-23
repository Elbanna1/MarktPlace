using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using ServicesAbstraction;

namespace Services.Caching;

public sealed class PublicResponseCache : IPublicResponseCache
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> Gates = new();

    private readonly IMemoryCache _cache;
    private readonly ILookupCache _version;

    public PublicResponseCache(IMemoryCache cache, ILookupCache version)
    {
        _cache = cache;
        _version = version;
    }

    public async Task<TValue> GetOrCreateAsync<TValue>(
        string key,
        TimeSpan duration,
        Func<CancellationToken, Task<TValue>> factory,
        CancellationToken cancellationToken = default)
    {
        var versionedKey = $"{key}:v{_version.Version}";

        if (_cache.TryGetValue(versionedKey, out TValue? cached) && cached is not null)
            return cached;

        var gate = Gates.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        await gate.WaitAsync(cancellationToken);

        try
        {
            if (_cache.TryGetValue(versionedKey, out cached) && cached is not null)
                return cached;

            var value = await factory(cancellationToken);

            if (value is not null)
            {
                _cache.Set(versionedKey, value, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = duration,

                    Size = 1
                });
            }

            return value;
        }
        finally
        {
            gate.Release();
        }
    }
}
