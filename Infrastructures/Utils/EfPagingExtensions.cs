namespace Infrastructures.Utils;

public static class EfPagingExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page = 1, int pageSize = 10)
    {
        if (page <= 0) page = 1;

        if (pageSize <= 0) pageSize = 10;

        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
