using System.Linq.Expressions;

namespace Applications.Responses.Paginations;

public static class QueryableExtensions
{
    public static IQueryable<T> SortBy<T>(this IQueryable<T> query, string orderField, string direction)
    {
        var queryElementTypeParam = Expression.Parameter(typeof(T));
        
        var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderField);
       
        var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

        var orderBy = Expression.Call(typeof(Queryable), direction == "ASC" ? "OrderBy" : "OrderByDescending",
            new Type[] { typeof(T), memberAccess.Type }, query.Expression, Expression.Quote(keySelector));

        return query.Provider.CreateQuery<T>(orderBy);
    }
}