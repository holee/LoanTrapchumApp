namespace Domains.Models.Paging;

public sealed class PageRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    public int Offset => (PageNumber - 1) * PageSize;
}
