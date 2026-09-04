namespace Infrastructures.Repository.Extensions;

public static class ColumnValidationExtensions
{
    public static string BuildSafeColumns<T>(this string[]? columns)
    {
        if (columns is not { Length: > 0 })
            return "*";

        var validColumns = typeof(T)
            .GetProperties()
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var safeColumns = columns
            .Where(validColumns.Contains)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return safeColumns.Length > 0 ? string.Join(", ", safeColumns) : "*";
    }
}
