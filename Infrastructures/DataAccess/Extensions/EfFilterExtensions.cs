using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;

namespace Infrastructures.DataAccess.Extensions;

public enum FilterOperator
{
    // ===== Comparison =====
    Equal,                  // =
    NotEqual,               // <>
    GreaterThan,            // >
    GreaterThanOrEqual,     // >=
    LessThan,               // <
    LessThanOrEqual,        // <=

    // ===== String =====
    Contains,               // LIKE %x%
    NotContains,
    StartsWith,             // LIKE x%
    EndsWith,               // LIKE %x
    Like,                   // custom LIKE pattern
    NotLike,

    // ===== Null =====
    IsNull,                 // IS NULL
    IsNotNull,              // IS NOT NULL

    // ===== Range =====
    Between,                // BETWEEN a AND b
    NotBetween,

    // ===== Collection =====
    In,                     // IN (...)
    NotIn,                  // NOT IN (...)

    // ===== Boolean =====
    True,                   // = 1
    False,                  // = 0

    // ===== Advanced =====
    Exists,                 // EXISTS (subquery)
    NotExists
}

public record FilterCondition(string Field, object? Value, FilterOperator Operator = FilterOperator.Equal);

public static class EfFilterExtensions
{
    public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> query, params FilterCondition[] filters)
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

    private static Expression BuildExpression(FilterOperator op, object? value, MemberExpression left, ConstantExpression constant)
    {
        return op switch
        {
            FilterOperator.Equal => Expression.Equal(left, constant!),
            FilterOperator.NotEqual => Expression.NotEqual(left, constant!),
            FilterOperator.GreaterThan => Expression.GreaterThan(left, constant!),
            FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(left, constant!),
            FilterOperator.LessThan => Expression.LessThan(left, constant!),
            FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(left, constant!),
            FilterOperator.Contains => Call(left, nameof(string.Contains), constant!),
            FilterOperator.NotContains => Expression.Not(Call(left, nameof(string.Contains), constant!)),
            FilterOperator.StartsWith => Call(left, nameof(string.StartsWith), constant!),
            FilterOperator.EndsWith => Call(left, nameof(string.EndsWith), constant!),
            FilterOperator.Like => Call(left, nameof(string.Contains), constant!),
            FilterOperator.NotLike => Expression.Not(Call(left, nameof(string.Contains), constant!)),
            FilterOperator.IsNull => Expression.Equal(left, Expression.Constant(null, left.Type)),
            FilterOperator.IsNotNull => Expression.NotEqual(left, Expression.Constant(null, left.Type)),
            FilterOperator.True => Expression.Equal(left, Expression.Constant(true)),
            FilterOperator.False => Expression.Equal(left, Expression.Constant(false)),
            FilterOperator.In => BuildIn(left, (IEnumerable)value!),
            FilterOperator.NotIn => Expression.Not(BuildIn(left, (IEnumerable)value!)),
            FilterOperator.Between => BuildBetween(left, (object[])value!),
            FilterOperator.NotBetween => Expression.Not(BuildBetween(left, (object[])value!)),
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
    static Expression BuildBetween(Expression left, object[] values)
    {
        var low = Expression.Constant(values[0], left.Type);
        var high = Expression.Constant(values[1], left.Type);

        return Expression.AndAlso(
            Expression.GreaterThanOrEqual(left, low),
            Expression.LessThanOrEqual(left, high));
    }
}
