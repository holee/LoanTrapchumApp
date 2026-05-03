using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructures.Utils;

public static class EfDtoProjectionExtensions
{
    private static readonly ConcurrentDictionary<string, object> _cache = new();

    public static IQueryable<TDest> ProjectTo<TSource, TDest>(this IQueryable<TSource> query, params string[]? columns)
        where TDest : class, new()
    {
        var key = $"{typeof(TSource).FullName}->{typeof(TDest).FullName}:{string.Join(",", columns ?? Array.Empty<string>())}";

        if (_cache.TryGetValue(key, out var cached))
            return query.Select((Expression<Func<TSource, TDest>>)cached);

        var sourceParam = Expression.Parameter(typeof(TSource), "x");

        var sourceProps = typeof(TSource)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

        var destProps = typeof(TDest)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite);

        if (columns?.Length > 0)
        {
            var set = new HashSet<string>(columns, StringComparer.OrdinalIgnoreCase);
            destProps = destProps.Where(p => set.Contains(p.Name));
        }

        var bindings = new List<MemberBinding>();

        foreach (var dest in destProps)
        {
            if (!sourceProps.TryGetValue(dest.Name, out var source))
                continue;

            var sourceAccess = Expression.Property(sourceParam, source);

            bindings.Add(Expression.Bind(dest, sourceAccess));
        }

        if (!bindings.Any())
            return query.Select(_ => new TDest());

        var body = Expression.MemberInit(Expression.New(typeof(TDest)), bindings);

        var selector = Expression.Lambda<Func<TSource, TDest>>(body, sourceParam);

        _cache.TryAdd(key, selector);

        return query.Select(selector);
    }

    public static IQueryable<T> ProjectTo<T>(this IQueryable<T> query, params string[] columns) where T : class, new()
    {
        if (columns == null || columns.Length == 0)
            return query;

        // normalize + cache key
        var key = $"{typeof(T).FullName}:{string.Join(",", columns.OrderBy(x => x))}";

        if (_cache.TryGetValue(key, out var cached))
            return query.Select((Expression<Func<T, T>>)cached);

        var parameter = Expression.Parameter(typeof(T), "x");

        var bindings = new List<MemberBinding>();

        var props = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var column in columns.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            // 🔥 silently skip if not found
            if (!props.TryGetValue(column, out var property))
                continue;

            // skip read-only
            if (!property.CanWrite)
                continue;

            var propertyAccess = Expression.Property(parameter, property);

            bindings.Add(Expression.Bind(property, propertyAccess));
        }

        // if nothing matched → return original query (avoid SELECT new T {})
        if (bindings.Count == 0)
            return query;

        var body = Expression.MemberInit(
            Expression.New(typeof(T)),
            bindings);

        var selector = Expression.Lambda<Func<T, T>>(body, parameter);

        _cache.TryAdd(key, selector);

        return query.Select(selector);
    }
}
