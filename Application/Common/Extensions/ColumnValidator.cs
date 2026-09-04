using System.Collections.Concurrent;

namespace Application.Extensions;

public static class ColumnValidator
{
    private static readonly ConcurrentDictionary<Type, HashSet<string>> PropertyNameCache = new();

    public static IReadOnlyList<string> GetValidColumns<T>(params IEnumerable<string> columns)
        => Filter(GetValidPropertyNames<T>(), columns);

    public static IReadOnlyList<string> GetValidColumns<T>(this T _, params string[] columns)
        => Filter(GetValidPropertyNames<T>(), columns);

    private static HashSet<string> GetValidPropertyNames<T>()
        => PropertyNameCache.GetOrAdd(
            typeof(T),
            type => type
                .GetProperties()
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase));

    private static IReadOnlyList<string> Filter(HashSet<string> validProperties, IEnumerable<string> columns)
        => columns
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c.Trim())
            .Where(validProperties.Contains) // discard invalid columns
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
}
