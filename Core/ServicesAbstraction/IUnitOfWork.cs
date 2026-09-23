namespace ServicesAbstraction;

public interface IUnitOfWork
{
    Task<ITransactionScope> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<TResult> ExecuteInTransactionAsync<TResult>(
        Func<ITransactionScope, CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken = default);

    bool IsUniqueConstraintViolation(Exception exception);
}

public interface ITransactionScope : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}
