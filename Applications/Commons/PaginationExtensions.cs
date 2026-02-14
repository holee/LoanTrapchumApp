using Microsoft.EntityFrameworkCore;
namespace Applications.Commons;

public static class PaginationExtensions
{
    public static async Task<PaginationResult<T>> CreateAsync<T>(IQueryable<T> query, int page, int pageSize, CancellationToken cancellation = default)
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

        return new PaginationResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
