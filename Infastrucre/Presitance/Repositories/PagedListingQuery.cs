using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public static class PagedListingQuery
{
    public static async Task<List<TEntity>> ToPageAsync<TEntity>(
        IQueryable<TEntity> ordered,
        int pageIndex,
        int pageSize,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> include,
        CancellationToken cancellationToken = default,
        string keyName = "Id")
        where TEntity : class
    {
        var skip = (pageIndex - 1) * pageSize;

        var keys = await ordered
            .Skip(skip)
            .Take(pageSize)
            .Select(entity => EF.Property<Guid>(entity, keyName))
            .ToListAsync(cancellationToken);

        if (keys.Count == 0)
            return new List<TEntity>();

        return await include(ordered.Where(entity => keys.Contains(EF.Property<Guid>(entity, keyName))))
            .ToListAsync(cancellationToken);
    }

    public static Task<List<TEntity>> ToStripAsync<TEntity>(
        IQueryable<TEntity> ordered,
        int count,
        Func<IQueryable<TEntity>, IQueryable<TEntity>> include,
        CancellationToken cancellationToken = default,
        string keyName = "Id")
        where TEntity : class =>
        ToPageAsync(ordered, pageIndex: 1, pageSize: count, include, cancellationToken, keyName);
}
