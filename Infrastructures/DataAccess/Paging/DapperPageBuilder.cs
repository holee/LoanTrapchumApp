using Dapper;
using Domains.Models.Paging;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructures.DataAccess.Paging;

public static class DapperPageBuilder 
{ 
    public static async Task<PagedResult<T>> CreatePagedAsync<T>(this IDbConnection db, string table, Expression<Func<T, bool>> filter, PageRequest page, params IEnumerable<string> columns)
    {
        var (whereSql, parameters) = filter.ToSql<T>(); 

        var sqlColumns = columns != null && columns.Any() ? string.Join(", ", columns.SelectMany(c => c)) : "*";

        var sortDirection = page.SortDirection switch
        {
            SortDirection.Ascending => "ASC",
            SortDirection.Descending => "DESC",
            _ => "ASC"
        };

        var sortBy= string.IsNullOrWhiteSpace(page.SortBy) ? "1" : page.SortBy;
        var sql = $@"
        SELECT {sqlColumns}
        FROM {table}
        WHERE {whereSql}
        ORDER BY {sortBy} {sortDirection}
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        SELECT COUNT(*)
        FROM {table}
        WHERE {whereSql};";

        parameters.Add("Offset", page.Offset);
        parameters.Add("PageSize", page.PageSize);

        using var multi = await db.QueryMultipleAsync(sql, parameters);

        return new PagedResult<T>
        {
            Items = (await multi.ReadAsync<T>()).ToList(),
            TotalCount = await multi.ReadSingleAsync<int>(),
            PageNumber = page.PageNumber,
            PageSize = page.PageSize
        };
    }

}
