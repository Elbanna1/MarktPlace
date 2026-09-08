namespace ServicesAbstraction;

public sealed record AccountClosureResult(int ListingsHidden, int InterestsRemoved);

public interface IAccountClosureRepository
{
    Task<AccountClosureResult> CloseAsync(string userId, CancellationToken cancellationToken = default);
}
