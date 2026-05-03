using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructures.Utils;

public enum SortDirection
{
    Ascending,
    Descending
}

public static class EfSortExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, params (string Property, SortDirection Direction)[] sorts)
    {
        if (sorts == null || sorts.Length == 0)
            return query;

        bool first = true;

        foreach (var (propertyName, direction) in sorts)
        {
            var property = typeof(T).GetProperty(
                propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
                continue;

            var parameter = Expression.Parameter(typeof(T), "x");
            var body = Expression.Property(parameter, property);
            var selector = Expression.Lambda(body, parameter);

            string methodName;

            if (first)
            {
                methodName = direction == SortDirection.Descending
                    ? nameof(Queryable.OrderByDescending)
                    : nameof(Queryable.OrderBy);

                first = false;
            }
            else
            {
                methodName = direction == SortDirection.Descending
                    ? nameof(Queryable.ThenByDescending)
                    : nameof(Queryable.ThenBy);
            }

            query = (IQueryable<T>)typeof(Queryable)
                .GetMethods()
                .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.PropertyType)
                .Invoke(null, new object[] { query, selector })!;
        }

        return query;
    }
}
