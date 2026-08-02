using Applications.Responses.ViewModels.Employees;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructures.Implements.Extensions;

public enum TextOperator
{
    Equals,
    Contains,
    StartsWith,
    EndsWith
}

public static class EfSearchExtensions
{

    public static IQueryable<T> StartWiths<T>(this IQueryable<T> source, string? filter, params string[] properties)
    {
        if (string.IsNullOrWhiteSpace(filter) || properties.Length == 0)
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? body = null;

        var efFunctions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))!);

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(nameof(DbFunctionsExtensions.Like),
                         new[] { typeof(DbFunctions), typeof(string), typeof(string) });

        foreach (var propName in properties)
        {
            var propInfo = typeof(T).GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propInfo == null || propInfo.PropertyType != typeof(string)) continue;

            var property = Expression.Property(parameter, propInfo);

            var coalesce = Expression.Coalesce(property, Expression.Constant(""));

            var pattern = Expression.Constant($"{filter}%");

            var likeCall = Expression.Call(null, likeMethod!, efFunctions, coalesce, pattern);

            body = body == null ? likeCall : Expression.OrElse(body, likeCall);
        }

        if (body == null)
            return source;

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return source.Where(lambda);
    }

    public static IQueryable<T> StartWith<T>(this IQueryable<T> source, string? filter, string columnName)
    {
        if (string.IsNullOrWhiteSpace(filter) || string.IsNullOrWhiteSpace(columnName))
            return source;

        return source.Where(e => EF.Functions.Like(EF.Property<string>(e!, columnName), $"{filter}%"));
    }

    public static IQueryable<T> Contains<T>(this IQueryable<T> source, string? filter, string property)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");

        var propInfo = typeof(T).GetProperty(
            property,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propInfo == null || propInfo.PropertyType != typeof(string))
            return source;

        var propertyAccess = Expression.Property(parameter, propInfo);

        var efFunctions = Expression.Property(
            null,
            typeof(EF).GetProperty(nameof(EF.Functions))!
        );

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
        )!;

        var pattern = Expression.Constant($"%{filter}%");

        var likeCall = Expression.Call(
            null,
            likeMethod,
            efFunctions,
            propertyAccess,
            pattern
        );

        var lambda = Expression.Lambda<Func<T, bool>>(likeCall, parameter);

        return source.Where(lambda);
    }

    public static IQueryable<T> AndContains<T>(this IQueryable<T> source, string? filter, params string[] properties)
    {
        if (string.IsNullOrWhiteSpace(filter) || properties.Length == 0)
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? body = null;

        var efFunctions = Expression.Property(
            null,
            typeof(EF).GetProperty(nameof(EF.Functions))!
        );

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
        )!;

        foreach (var propName in properties)
        {
            var propInfo = typeof(T).GetProperty(
                propName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );

            if (propInfo == null || propInfo.PropertyType != typeof(string))
                continue;

            var property = Expression.Property(parameter, propInfo);
            var coalesce = Expression.Coalesce(property, Expression.Constant(""));
            var pattern = Expression.Constant($"%{filter}%");

            var likeCall = Expression.Call(
                null,
                likeMethod,
                efFunctions,
                coalesce,
                pattern
            );

            body = body == null ? likeCall : Expression.AndAlso(body, likeCall);
        }

        if (body == null)
            return source;

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        return source.Where(lambda);
    }

    public static IQueryable<T> AndStartWith<T>(this IQueryable<T> source, string? filter, params string[] properties)
    {
        if (string.IsNullOrWhiteSpace(filter) || properties.Length == 0)
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? body = null;

        var efFunctions = Expression.Property(
            null,
            typeof(EF).GetProperty(nameof(EF.Functions))!
        );

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
        )!;

        foreach (var propName in properties)
        {
            var propInfo = typeof(T).GetProperty(
                propName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );

            if (propInfo == null || propInfo.PropertyType != typeof(string))
                continue;

            var property = Expression.Property(parameter, propInfo);
            var coalesce = Expression.Coalesce(property, Expression.Constant(""));
            var pattern = Expression.Constant($"{filter}%");

            var likeCall = Expression.Call(
                null,
                likeMethod,
                efFunctions,
                coalesce,
                pattern
            );

            body = body == null ? likeCall : Expression.AndAlso(body, likeCall);
        }

        if (body == null)
            return source;

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        return source.Where(lambda);
    }

    public static IQueryable<T> AndStartWith<T>(this IQueryable<T> source, string? filter, string? property)
    {
        if (string.IsNullOrWhiteSpace(filter) || string.IsNullOrEmpty(property))
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? body = null;

        var efFunctions = Expression.Property(
            null,
            typeof(EF).GetProperty(nameof(EF.Functions))!
        );

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
        )!;


        var propInfo = typeof(T).GetProperty(
            property,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
        );

        if (propInfo is null || propInfo.PropertyType != typeof(string))
        {
            return source;
        }

        var prop = Expression.Property(parameter, propInfo);

        var coalesce = Expression.Coalesce(prop, Expression.Constant(""));

        var pattern = Expression.Constant($"{filter}%");

        var likeCall = Expression.Call(
            null,
            likeMethod,
            efFunctions,
            coalesce,
            pattern
        );

        body = body == null ? likeCall : Expression.AndAlso(body, likeCall);

        if (body == null)
            return source;

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return source.Where(lambda);
    }

    public static IQueryable<T> AndEndWith<T>(this IQueryable<T> source, string? filter, params string[] properties)
    {
        if (string.IsNullOrWhiteSpace(filter) || properties.Length == 0)
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? body = null;

        var efFunctions = Expression.Property(
            null,
            typeof(EF).GetProperty(nameof(EF.Functions))!
        );

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
        )!;

        foreach (var propName in properties)
        {
            var propInfo = typeof(T).GetProperty(
                propName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );

            if (propInfo == null || propInfo.PropertyType != typeof(string))
                continue;

            var property = Expression.Property(parameter, propInfo);
            var coalesce = Expression.Coalesce(property, Expression.Constant(""));
            var pattern = Expression.Constant($"%{filter}");

            var likeCall = Expression.Call(
                null,
                likeMethod,
                efFunctions,
                coalesce,
                pattern
            );

            body = body == null ? likeCall : Expression.AndAlso(body, likeCall);

        }

        if (body == null)
            return source;

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return source.Where(lambda);
    }

    public static IQueryable<T> WhereText<T>(this IQueryable<T> source, string? filter, TextOperator op,
    bool useOr, bool ignoreCase, params string[] properties)
    {
        if (string.IsNullOrWhiteSpace(filter) || properties.Length == 0)
            return source;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? body = null;

        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;
        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
        )!;

        var efFunctions = Expression.Property(
            null,
            typeof(EF).GetProperty(nameof(EF.Functions))!
        );

        foreach (var propName in properties)
        {
            var propInfo = typeof(T).GetProperty(
                propName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
            );

            if (propInfo == null || propInfo.PropertyType != typeof(string))
                continue;

            Expression property = Expression.Property(parameter, propInfo);
            property = Expression.Coalesce(property, Expression.Constant(""));

            if (ignoreCase)
                property = Expression.Call(property, toLowerMethod);

            var value = ignoreCase ? filter.ToLower() : filter;

            Expression predicate = op switch
            {
                TextOperator.Equals =>
                    Expression.Equal(property, Expression.Constant(value)),

                TextOperator.Contains =>
                    Expression.Call(
                        null,
                        likeMethod,
                        efFunctions,
                        property,
                        Expression.Constant($"%{value}%")
                    ),

                TextOperator.StartsWith =>
                    Expression.Call(
                        null,
                        likeMethod,
                        efFunctions,
                        property,
                        Expression.Constant($"{value}%")
                    ),

                TextOperator.EndsWith =>
                    Expression.Call(
                        null,
                        likeMethod,
                        efFunctions,
                        property,
                        Expression.Constant($"%{value}")
                    ),

                _ => throw new NotSupportedException()
            };

            body = body == null
                ? predicate
                : useOr
                    ? Expression.OrElse(body, predicate)
                    : Expression.AndAlso(body, predicate);
        }

        if (body == null)
            return source;

        return source.Where(Expression.Lambda<Func<T, bool>>(body, parameter));
    }

    public static IQueryable<T> OrWhere<T>(this IQueryable<T> source, string? filter, params string[] properties)
    => source.WhereText(
        filter,
        TextOperator.Contains,
        useOr: true,
        false,
        properties
    );

    public static IQueryable<T> OrStartsWith<T>(this IQueryable<T> source, string? filter, params string[] properties)
        => source.WhereText(
            filter,
            TextOperator.StartsWith,
            useOr: true,
            false,
            properties
        );

    public static IQueryable<T> OrEndsWith<T>(this IQueryable<T> source, string? filter, params string[] properties)
        => source.WhereText(
            filter,
            TextOperator.EndsWith,
            useOr: true,
            false,
            properties
        );



    public static string? MapField(this Type type, string? propertyName) =>
        propertyName is not null && type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) is not null
            ? propertyName
            : null;

    /// <summary>
    /// Orders the query by the given column name (via <see cref="EF.Property{TProperty}"/>) when it maps
    /// to a valid property on <typeparamref name="T"/>; otherwise falls back to <paramref name="defaultKeySelector"/>.
    /// </summary>
    public static IOrderedQueryable<T> OrderByColumn<T>(
        this IQueryable<T> source,
        string? sortColumn,
        bool sortDescending,
        Expression<Func<T, object?>> defaultKeySelector)
    {
        var column = typeof(T).MapField(sortColumn);

        if (string.IsNullOrEmpty(column))
        {
            return sortDescending
                ? source.OrderByDescending(defaultKeySelector)
                : source.OrderBy(defaultKeySelector);
        }

        return sortDescending
            ? source.OrderByDescending(x => EF.Property<object>(x!, column))
            : source.OrderBy(x => EF.Property<object>(x!, column));
    }
}










//var parameter = Expression.Parameter(typeof(VCustomerList), "x");

//Expression? body = null;

//foreach (var prop in props)
//{
//    var property = Expression.Property(parameter, prop);
//    var coalesce = Expression.Coalesce(property, Expression.Constant(""));

//    var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
//                        nameof(DbFunctionsExtensions.Like),
//                        new[] { typeof(DbFunctions), typeof(string), typeof(string) });

//    var call = Expression.Call(
//        null,
//        likeMethod!,
//        Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions))!),
//        coalesce,
//        Expression.Constant($"{filter}%")
//    );

//    body = body == null ? call : Expression.OrElse(body, call);
//}

//var lambda = Expression.Lambda<Func<VCustomerList, bool>>(body!, parameter);
//queries = queries.Where(lambda);