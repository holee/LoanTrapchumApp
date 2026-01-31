using Dapper;
using System.Linq.Expressions;

namespace Infrastructures.DataAccess.Paging;

public static class ExpressionExtensions
{
    public static (string Sql, DynamicParameters Params) ToSql<T>(this Expression<Func<T, bool>> expression)
    {
        var visitor = new SqlExpressionVisitor();
        return visitor.Translate(expression.Body);
    }

    public static (string Sql, DynamicParameters Params) ToSql<T>(this Expression<Func<T, bool>> expression, string tableName)
    {
        var visitor = new SqlExpressionVisitor();
        return visitor.Translate(expression.Body);
    }
}
