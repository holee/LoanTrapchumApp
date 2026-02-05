namespace Infrastructures.DataAccess.SqlBuilders;

public static class SqlColumnHelper
{
    public static string Columns<T>(params string[]? columns)
    {
        if (columns == null || columns.Length == 0)
            return "*";

        var valid = typeof(T)
            .GetProperties()
            .Select(x => x.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var safe = columns
            .Where(valid.Contains)
            .Distinct(StringComparer.OrdinalIgnoreCase);

        return string.Join(",", safe);
    }
}
