namespace Applications.Commons.Extensions.QueryableExtensions
{

    public static class QueryableExtensions
    {
        public static IQueryable<TDestination> ProjectTo<TSource, TDestination>(this IQueryable<TSource> source)
            where TDestination : class, new()
        {
            if (typeof(TDestination).IsAnonymousType())
            {
                throw new InvalidOperationException("Anonymous types are not supported.");
            }

            var selector = ProjectionBuilder.GetMapExpression<TSource, TDestination>();
            return source.Select(selector);
        }
    }

}
