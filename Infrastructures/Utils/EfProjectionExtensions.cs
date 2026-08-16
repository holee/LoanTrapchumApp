using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructures.Utils;

public static class EfProjectionExtensions
{
    private static readonly ConcurrentDictionary<string, object> ExpressionCache = new();

    private static readonly ConcurrentDictionary<Type, FrozenDictionary<string, PropertyInfo>> PropertyCache = new();

    public static IQueryable<T> ProjectTo<T>(this IQueryable<T> query, params string[] columns)
        where T : class, new()
    {
        if (columns is not { Length: > 0 })
            return query;

        var props = GetWritableProperties(typeof(T));

        // ── discard columns that don't match any property ───────────
        var matched = columns
            .Where(c => props.ContainsKey(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (matched.Length == 0)
            return query;

        // ── build a deterministic, case-insensitive cache key ───────
        var sortedLower = matched.Select(c => c.ToLowerInvariant()).ToArray();
        Array.Sort(sortedLower, StringComparer.Ordinal);

        var key = string.Concat(typeof(T).FullName, ":", string.Join(',', sortedLower));

        // ── fast path: cached expression ────────────────────────────
        if (ExpressionCache.TryGetValue(key, out var cached))
            return query.Select((Expression<Func<T, T>>)cached);

        // ── slow path: build expression tree ────────────────────────
        var parameter = Expression.Parameter(typeof(T), "x");
        var bindings = new List<MemberBinding>(matched.Length);

        foreach (var col in matched)
        {
            var pi = props[col];
            bindings.Add(Expression.Bind(pi, Expression.Property(parameter, pi)));
        }



        var body = Expression.MemberInit(Expression.New(typeof(T)), bindings);
        var selector = Expression.Lambda<Func<T, T>>(body, parameter);

        ExpressionCache.TryAdd(key, selector);

        return query.Select(selector);
    }

    public static IQueryable<TDest> MapToType<TSource, TDest>(this IQueryable<TSource> query,
        params string[]? columns) where TDest : class, new()
    {
        // ── cache key ───────────────────────────────────────────────
        string suffix;

        var sourceProps = GetWritableProperties(typeof(TSource));

        var destProps = GetWritableProperties(typeof(TDest));

        // ── discard columns that don't exist on both source and dest ─
        HashSet<string>? requestedSet = null;

        if (columns is { Length: > 0 })
        {
            requestedSet = new HashSet<string>(
                columns.Where(c => destProps.ContainsKey(c) && sourceProps.ContainsKey(c)),
                StringComparer.OrdinalIgnoreCase);
        }

        if (columns is { Length: > 0 })
        {
            var sortedLower = requestedSet!.Select(c => c.ToLowerInvariant()).OrderBy(c => c, StringComparer.Ordinal);
            suffix = string.Join(',', sortedLower);
        }
        else
        {
            suffix = "*";
        }

        var key = string.Concat(typeof(TSource).FullName, "->", typeof(TDest).FullName, ":", suffix);

        // ── fast path ───────────────────────────────────────────────
        if (ExpressionCache.TryGetValue(key, out var cached))
            return query.Select((Expression<Func<TSource, TDest>>)cached);

        // ── slow path ───────────────────────────────────────────────
        var parameter = Expression.Parameter(typeof(TSource), "x");

        var bindings = new List<MemberBinding>(requestedSet?.Count ?? destProps.Count);

        foreach (var (name, destPi) in destProps)
        {
            if (requestedSet is not null && !requestedSet.Contains(name))
                continue;

            if (!sourceProps.TryGetValue(name, out var sourcePi))
                continue;

            if (destPi.PropertyType != sourcePi.PropertyType)
                continue;

            bindings.Add(Expression.Bind(destPi, Expression.Property(parameter, sourcePi)));
        }

        if (bindings.Count == 0)
            return query.Select(_ => new TDest());

        var body = Expression.MemberInit(Expression.New(typeof(TDest)), bindings);

        var selector = Expression.Lambda<Func<TSource, TDest>>(body, parameter);

        ExpressionCache.TryAdd(key, selector);

        return query.Select(selector);
    }

    private static FrozenDictionary<string, PropertyInfo> GetWritableProperties(Type type)
    {
        return PropertyCache.GetOrAdd(type, static t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
             .Where(p => p.CanWrite)
             .ToFrozenDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase));
    }
}