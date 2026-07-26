using Microsoft.EntityFrameworkCore;
namespace Applications.Commons;

public static class PaginationExtensions
{
    public static async Task<PageMetaData<T>> CreateAsync<T>(IQueryable<T> query, int page, int pageSize, CancellationToken cancellation = default)
    {
        int totalCount = await query.CountAsync(cancellation);

        if (pageSize > 0)
        {
            query = query.Skip((page - 1) * pageSize)
                               .Take(pageSize);
        }

        var items = await query
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync(cancellation);

        return new PageMetaData<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public static async Task<PageMetaData<T>> ToPagedAsync<T>(this IQueryable<T> query, int page, int pageSize,
      CancellationToken cancellationToken = default)
    {
        if (page < 0)
            throw new ArgumentOutOfRangeException(nameof(page));

        if (pageSize < 0)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        var totalCount = await query.CountAsync(cancellationToken);

        if (pageSize == 0)
        {
            var allItems = await query.ToListAsync(cancellationToken);

            return new PageMetaData<T>
            {
                Items = allItems,
                Page = 1,
                PageSize = totalCount,
                TotalCount = totalCount
            };
        }

        var items = await query
            .Skip((page == 0 ? 1 : page - 1) * pageSize)
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
