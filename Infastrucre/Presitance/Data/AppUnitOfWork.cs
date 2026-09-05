using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ServicesAbstraction;

namespace Persistence.Data;

public class AppUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public AppUnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ITransactionScope> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new EfTransactionScope(transaction);
    }

    public bool IsUniqueConstraintViolation(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is SqlException sql && sql.Number is 2601 or 2627)
                return true;
        }

        return false;
    }

    private sealed class EfTransactionScope : ITransactionScope
    {
        private readonly IDbContextTransaction _transaction;

        public EfTransactionScope(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default) =>
            _transaction.CommitAsync(cancellationToken);

        public Task RollbackAsync(CancellationToken cancellationToken = default) =>
            _transaction.RollbackAsync(cancellationToken);

        public ValueTask DisposeAsync() => _transaction.DisposeAsync();
    }
}
