using Microsoft.Extensions.Caching.Memory;
using ServicesAbstraction;

namespace Persistence.Services;

public sealed class UserRoleCache : IUserRoleCache, IDisposable
{
    private static readonly TimeSpan Lifetime = TimeSpan.FromMinutes(5);

    private const long Capacity = 20_000;

    private readonly MemoryCache _cache = new(new MemoryCacheOptions { SizeLimit = Capacity });

    public bool TryGet(string userId, out IReadOnlyList<string> roles)
    {
        if (_cache.TryGetValue(Key(userId), out IReadOnlyList<string>? cached) && cached is not null)
        {
            roles = cached;
            return true;
        }

        roles = Array.Empty<string>();
        return false;
    }

    public void Set(string userId, IReadOnlyList<string> roles) =>
        _cache.Set(
            Key(userId),
            roles,
            new MemoryCacheEntryOptions { Size = 1, AbsoluteExpirationRelativeToNow = Lifetime });

    public void Invalidate(string userId) => _cache.Remove(Key(userId));

    public void Dispose() => _cache.Dispose();

    private static string Key(string userId) => $"user-roles:{userId}";
}
