namespace Infrastructures.DataAccess.SqlBuilders;

public static class SqlPaging
{
    public static string Apply(string sql, int page, int size)
    {
        var offset = (page - 1) * size;

        return $"""
            {sql}
            OFFSET {offset} ROWS
            FETCH NEXT {size} ROWS ONLY
            """;
    }
}
