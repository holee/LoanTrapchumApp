using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string? sortColumn, bool sortDescending,string? defaultSortColumn1 = null, string? defaultSortColumn2 = null)
        {
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                var property = typeof(T).GetProperty(sortColumn);

                if (property != null)
                {
                    // Use EF.Property for dynamic column ordering
                    query = sortDescending
                        ? query.OrderByDescending(e => EF.Property<object>(e!, sortColumn))
                        : query.OrderBy(e => EF.Property<object>(e!, sortColumn));
                }
            }
            else
            {
                // fallback sorting if column not provided
                if (!string.IsNullOrEmpty(defaultSortColumn1))
                {
                    query = query.OrderBy(e => EF.Property<object>(e!, defaultSortColumn1));

                    if (!string.IsNullOrEmpty(defaultSortColumn2))
                    {
                        query = ((IOrderedQueryable<T>)query)
                            .ThenBy(e => EF.Property<object>(e!, defaultSortColumn2));
                    }
                }
            }

            return query;
        }
    }

}
