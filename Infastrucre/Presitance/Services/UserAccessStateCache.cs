using Microsoft.Extensions.Caching.Memory;
using ServicesAbstraction;

namespace Persistence.Services;

public sealed class UserAccessStateCache : IUserAccessStateCache, IDisposable
{
    private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(5);

    private const long Capacity = 20_000;

    private readonly MemoryCache _cache = new(new MemoryCacheOptions { SizeLimit = Capacity });

    public bool TryGet(string userId, out UserAccessState state)
    {
        if (_cache.TryGetValue(Key(userId), out UserAccessState? cached) && cached is not null)
        {
            state = cached;
            return true;
        }

        state = UserAccessState.Missing;
        return false;
    }

    public void Set(string userId, UserAccessState state) =>
        _cache.Set(
            Key(userId),
            state,
            new MemoryCacheEntryOptions { Size = 1, AbsoluteExpirationRelativeToNow = Lifetime });

    public void Invalidate(string userId) => _cache.Remove(Key(userId));

    public void Dispose() => _cache.Dispose();

    private static string Key(string userId) => $"user-access-state:{userId}";
}
