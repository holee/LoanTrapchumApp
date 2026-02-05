namespace Infrastructures.DataAccess.SqlBuilders;

public static class SqlSortBuilder
{
    public static string OrderBy<T>(string? column, bool desc = false)
    {
        if (string.IsNullOrWhiteSpace(column))
            return "ORDER BY (SELECT 1)";

        var valid = typeof(T).GetProperties()
            .Any(p => p.Name.Equals(column, StringComparison.OrdinalIgnoreCase));

        if (!valid)
            return "ORDER BY (SELECT 1)";

        return $"ORDER BY {column} {(desc ? "DESC" : "ASC")}";
    }
}
