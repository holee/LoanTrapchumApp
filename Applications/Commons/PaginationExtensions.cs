using Microsoft.EntityFrameworkCore;
namespace Applications.Commons;

public static class PaginationExtensions
{

    public static async Task<PageMetaData<T>> ToPagedAsync<T>(this IQueryable<T> query,
      int page,
      int pageSize,
      CancellationToken cancellationToken = default)
    {
        if (pageSize < 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        var totalCount = await query.CountAsync(cancellationToken);

        if (page == 0 && pageSize == 0)
        {
            var allItems = await query.ToListAsync(cancellationToken);

            return new PageMetaData<T>
            {
                Items = allItems,
                Page = 0,
                PageSize = totalCount,
                TotalCount = totalCount
            };
        }

        var skip = page == 0 ? 0 : checked((page - 1) * pageSize);

        var items = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PageMetaData<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
