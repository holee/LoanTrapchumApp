using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructures.Utils;

public static class EfFilterExtensions
{
    public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> query, params Filters[] filters)
    {
        if (filters == null || filters.Length == 0)
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? final = null;

        foreach (var filter in filters)
        {
            var property = typeof(T).GetProperty(
                filter.Field,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                continue;

            var left = Expression.Property(parameter, property);

            var constant = Expression.Constant(
                Convert.ChangeType(filter.Value, property.PropertyType),
                property.PropertyType);

            Expression comparison = BuildExpression(filter.Operator, filter.Value, left, constant);

            if (comparison == null)
                continue;

            final = final == null
                ? comparison
                : Expression.AndAlso(final, comparison);
        }

        if (final == null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(final, parameter);

        return query.Where(lambda);
    }

    private static Expression BuildExpression(FilterOp op, object? value, MemberExpression left, ConstantExpression constant)
    {
        return op switch
        {
            FilterOp.Equal => Expression.Equal(left, constant!),
            FilterOp.NotEqual => Expression.NotEqual(left, constant!),
            FilterOp.GreaterThan => Expression.GreaterThan(left, constant!),
            FilterOp.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, constant!),
            FilterOp.LessThan => Expression.LessThan(left, constant!),
            FilterOp.LessThanOrEqual => Expression.LessThanOrEqual(left, constant!),
            FilterOp.Contains => Call(left, nameof(string.Contains), constant!),
            FilterOp.NotContains => Expression.Not(Call(left, nameof(string.Contains), constant!)),
            FilterOp.StartsWith => Call(left, nameof(string.StartsWith), constant!),
            FilterOp.EndsWith => Call(left, nameof(string.EndsWith), constant!),
            FilterOp.Like => Call(left, nameof(string.Contains), constant!),
            FilterOp.NotLike => Expression.Not(Call(left, nameof(string.Contains), constant!)),
            FilterOp.IsNull => Expression.Equal(left, Expression.Constant(null, left.Type)),
            FilterOp.IsNotNull => Expression.NotEqual(left, Expression.Constant(null, left.Type)),
            FilterOp.True => Expression.Equal(left, Expression.Constant(true)),
            FilterOp.False => Expression.Equal(left, Expression.Constant(false)),
            FilterOp.In => BuildIn(left, (IEnumerable)value!),
            FilterOp.NotIn => Expression.Not(BuildIn(left, (IEnumerable)value!)),
            FilterOp.Between => BuildBetween(left, (object[])value!),
            FilterOp.NotBetween => Expression.Not(BuildBetween(left, (object[])value!)),
            _ => throw new NotSupportedException(op.ToString())
        };
    }

    private static Expression Call(Expression left, string method, Expression arg) => Expression.Call(left, method, Type.EmptyTypes, arg);
    
    private static Expression BuildIn(Expression left, IEnumerable values)
    {
        var method = typeof(Enumerable)
            .GetMethods()
            .First(x => x.Name == nameof(Enumerable.Contains)
                     && x.GetParameters().Length == 2)
            .MakeGenericMethod(left.Type);

        return Expression.Call(
            method,
            Expression.Constant(values),
            left);
    }
    
    private static Expression BuildBetween(Expression left, object[] values)
    {
        var low = Expression.Constant(values[0], left.Type);
        var high = Expression.Constant(values[1], left.Type);

        return Expression.AndAlso(
            Expression.GreaterThanOrEqual(left, low),
            Expression.LessThanOrEqual(left, high));
    }
}
