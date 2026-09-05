using System.Linq.Expressions;
using Shared.Enums;

namespace Persistence.Repositories;

internal static class OnlineShoppingSorting
{
    public static IOrderedQueryable<T> Apply<T>(
        IQueryable<T> query,
        OnlineShoppingSortBy sortBy,
        Expression<Func<T, decimal>> price,
        Expression<Func<T, DateTime>> createdAt,
        Expression<Func<T, Guid>> id) =>
        sortBy switch
        {
            OnlineShoppingSortBy.Oldest => query.OrderBy(createdAt).ThenBy(id),
            OnlineShoppingSortBy.PriceAsc => query.OrderBy(price).ThenByDescending(createdAt).ThenByDescending(id),
            OnlineShoppingSortBy.PriceDesc => query.OrderByDescending(price).ThenByDescending(createdAt).ThenByDescending(id),

            _ => query.OrderByDescending(createdAt).ThenByDescending(id)
        };
}
