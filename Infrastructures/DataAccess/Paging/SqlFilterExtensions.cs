using Dapper;
using Domains.Models.Paging;

namespace Infrastructures.DataAccess.Paging;

public static class SqlFilterExtensions
{
    public static (string whereSql, DynamicParameters parameters) ToSqlFilter(this IEnumerable<Filter> filters)
    {
        return SqlFilterBuilder.Build(filters);
    }
}
