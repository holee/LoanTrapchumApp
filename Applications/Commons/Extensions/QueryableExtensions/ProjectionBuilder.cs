using System.Linq.Expressions;

namespace Applications.Commons.Extensions.QueryableExtensions
{
    public static class ProjectionBuilder
    {
        public static Expression<Func<TSource, TDestination>> GetMapExpression<TSource, TDestination>()
            where TDestination : class, new()
        {
            var source = Expression.Parameter(typeof(TSource), "e");

            var bindings = typeof(TDestination).GetProperties()
                .Where(destProp => destProp.CanWrite)
                .Select(destProp =>
                {
                    var sourceProp = typeof(TSource).GetProperty(destProp.Name);
                    if (sourceProp == null || sourceProp.PropertyType != destProp.PropertyType)
                        return null;

                    return Expression.Bind(
                        destProp,
                        Expression.Property(source, sourceProp)
                    );
                })
                .Where(b => b != null)
                .ToArray();

            var body = Expression.MemberInit(Expression.New(typeof(TDestination)), bindings);
            return Expression.Lambda<Func<TSource, TDestination>>(body, source);
        }
    }

}
