namespace ServicesAbstraction;

public interface ILookupCache
{
    long Version { get; }

    void Invalidate();
}
