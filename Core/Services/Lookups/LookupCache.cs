using ServicesAbstraction;

namespace Services.Lookups;

public sealed class LookupCache : ILookupCache
{
    private long _version;

    public long Version => Interlocked.Read(ref _version);

    public void Invalidate() => Interlocked.Increment(ref _version);
}
