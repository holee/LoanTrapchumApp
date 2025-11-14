using Applications.Responses.Paginations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Implements.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, string? sortColumn, bool sortDescending, string? defaultSortColumn1 = null, string? defaultSortColumn2 = null)
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

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, QueryParam param, Expression<Func<T, object>>? defaultOrder = null)
        {
            if (!string.IsNullOrWhiteSpace(param.SortColumn))
            {
                bool isDescending = param.SortDescending;
                var property = typeof(T).GetProperty(param.SortColumn);
                if (property != null)
                {
                    query = isDescending
                        ? query.OrderByDescending(e => EF.Property<object>(e!, param.SortColumn))
                        : query.OrderBy(e => EF.Property<object>(e!, param.SortColumn));
                }
            }
            else if (defaultOrder != null)
            {
                query = query.OrderBy(defaultOrder);
            }

            return query;
        }

    }
}
