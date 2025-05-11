namespace Infrastructures.DataAccess
{
    using System.Linq.Expressions;

    public static class QueryableExtensions
    {
        public static IQueryable<T> OrWhere<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            var parameter = Expression.Parameter(typeof(T));

            var body = Expression.OrElse(
                Expression.Invoke(source.Expression, parameter),
                Expression.Invoke(predicate, parameter)
            );

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

            return source.Where(lambda);
        }
    }

}
