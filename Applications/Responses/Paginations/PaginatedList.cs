namespace Applications.Responses.Paginations;
public class PaginatedList<T>
{
    public PaginatedList()
    {
        PageIndex = 1;
        TotalPages= 10;
        FromItem = 1;
        ToItem = 10;
        Items = new List<T>();
    }
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public static int ItemsCount { get; set; }
    public int TotalItems { get; set; } = ItemsCount;
    public int FromItem { get; set; }
    public int ToItem { get; set; }
    public List<T> Items { get; set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        if (pageIndex == 1)
        {
            FromItem = 1;
            ToItem = pageSize < TotalItems ? pageSize : TotalItems;
        }
        else
        {
            var nextItems = pageSize * pageIndex;
            FromItem=((pageSize * (pageIndex - 1)) + 1);
            ToItem = nextItems < TotalItems ? nextItems : TotalItems; 
        }
        this.Items = new List<T>();
        this.Items.AddRange(items);
    }
    public bool HasPreviousPage=> PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        ItemsCount = source.ToList().Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return await Task.FromResult(new PaginatedList<T>(items, count, pageIndex, pageSize));
    }
}