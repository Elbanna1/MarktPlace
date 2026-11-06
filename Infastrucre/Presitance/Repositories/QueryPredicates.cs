using System.Linq.Expressions;

namespace Persistence.Repositories;

internal static class QueryPredicates
{
    public static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) =>
        Combine(left, right, Expression.AndAlso);

    public static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right) =>
        Combine(left, right, Expression.OrElse);

    public static Expression<Func<T, bool>> Not<T>(Expression<Func<T, bool>> predicate) =>
        Expression.Lambda<Func<T, bool>>(Expression.Not(predicate.Body), predicate.Parameters);

    private static Expression<Func<T, bool>> Combine<T>(
        Expression<Func<T, bool>> left, Expression<Func<T, bool>> right,
        Func<Expression, Expression, BinaryExpression> merge)
    {
        var parameter = left.Parameters[0];
        var rightBody = new ParameterSwap(right.Parameters[0], parameter).Visit(right.Body);

        return Expression.Lambda<Func<T, bool>>(merge(left.Body, rightBody), parameter);
    }

    private sealed class ParameterSwap : ExpressionVisitor
    {
        private readonly ParameterExpression _from;
        private readonly ParameterExpression _to;

        public ParameterSwap(ParameterExpression from, ParameterExpression to)
        {
            _from = from;
            _to = to;
        }

        protected override Expression VisitParameter(ParameterExpression node) =>
            node == _from ? _to : base.VisitParameter(node);
    }
}
