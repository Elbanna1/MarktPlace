namespace ServicesAbstraction;

public interface IListingLifecycleProcessor
{
    Task ProcessAsync(DateTime utcNow, CancellationToken cancellationToken = default);
}
