namespace ServicesAbstraction;

public interface IPublicResponseCache
{
    Task<TValue> GetOrCreateAsync<TValue>(
        string key,
        TimeSpan duration,
        Func<CancellationToken, Task<TValue>> factory,
        CancellationToken cancellationToken = default);
}
