using Dapper;
using Domains.Models.Paging;
using System.Data;

namespace Infrastructures.DataAccess.Paging;

public static class PageExtensions
{
    public static async Task<PagedResult<T>> GetPagedAsync<T>(this IDbConnection db, string tableName, PageRequest page, IEnumerable<Filter> filters)
    {

        var (whereSql, parameters) = SqlFilterBuilder.Build(filters);

        var sortColumn = string.IsNullOrWhiteSpace(page.SortBy) ? "1" : page.SortBy;

        var sql = $@"
        SELECT *
        FROM {tableName}
             {whereSql}
        ORDER BY {sortColumn} {page.SortDirection}
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(*)
        FROM {tableName}
        {whereSql};";

        parameters.Add("@Offset", page.Offset);

        parameters.Add("@PageSize", page.PageSize);

        using var multi = await db.QueryMultipleAsync(sql, parameters);

        var items = (await multi.ReadAsync<T>()).ToList();

        var totalCount = await multi.ReadSingleAsync<int>();

        return PagedResultExtensions.Create(items, page.PageNumber, page.PageSize, totalCount);
    }


    public static async Task<PagedResult<T>> GetPagedAsync<T>(this IDbConnection db, string tableName, PageRequest page, IEnumerable<Filter> filters, params string[] columns)
    {

        var (whereSql, parameters) = SqlFilterBuilder.Build(filters);

        var sortColumn = string.IsNullOrWhiteSpace(page.SortBy) ? "1" : page.SortBy;

        var sqlColumns = columns.Length == 0 ? "*" : string.Join(", ", columns);

        var sortDirection = page.SortDirection switch
        {
            SortDirection.Ascending => "ASC",
            SortDirection.Descending => "DESC",
            _ => "ASC"
        };

        var sql = $@"
        SELECT {sqlColumns}
        FROM {tableName}
            {whereSql}
        ORDER BY {sortColumn} {sortDirection}
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(*)
        FROM {tableName}
        {whereSql};";

        parameters.Add("@Offset", page.Offset);

        parameters.Add("@PageSize", page.PageSize);

        using var multi = await db.QueryMultipleAsync(sql, parameters);

        var items = (await multi.ReadAsync<T>()).ToList();

        var totalCount = await multi.ReadSingleAsync<int>();

        return PagedResultExtensions.Create(items, page.PageNumber, page.PageSize, totalCount);
    }

}