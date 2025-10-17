using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructures.Implements.Extensions;

public static class EfSearchExtensions
{

    public static IQueryable<T> WhereMatchesFilter<T>(this IQueryable<T> source,string? filter,params string[] properties)
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
            var propInfo = typeof(T).GetProperty(propName,BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

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