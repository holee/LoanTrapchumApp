namespace Application.Responses.Paginations
{
    public class SimplePagination<T>
    {
        private SimplePagination(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
        public List<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;
        public static async Task<SimplePagination<T>> Create(IQueryable<T> query, int page, int pageSize)
        {
            var totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return await Task.FromResult(new SimplePagination<T>(items, page, pageSize, totalCount));
        }

    }
}
