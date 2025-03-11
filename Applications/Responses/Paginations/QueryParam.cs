namespace Applications.Responses.Paginations;

public sealed class QueryParam
{
    public QueryParam()
    {
        PageIndex = 1;
        PageSize = 10;
        SortColumn = string.Empty;
        SortDescending = false;
        IsIncludeInActive = false;
        Filter = string.Empty;
    }
    public QueryParam(int pageIndex, int pageSize, string sortField)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        SortColumn = sortField;
    }
    public QueryParam(int pageIndex, int pageSize, string sortField, string? filter = null)
        : this(pageIndex, pageSize, sortField)
    {
        Filter = filter;
    }
    public QueryParam(int pageIndex, int pageSize, string sortField, bool sortDescending = false, string? filter = null)
        : this(pageIndex, pageSize, sortField)
    {
        SortDescending = sortDescending;
        Filter = filter;
    }
    public QueryParam(int pageIndex, int pageSize, string sortField,
        bool sortDescending = false,
        bool isIncludeInActive = false, string? filter = null) : this(pageIndex, pageSize, sortField)
    {
        SortDescending = sortDescending;
        IsIncludeInActive = isIncludeInActive;
        Filter = filter;
    }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "Id";
    public bool SortDescending { get; set; } = false;
    public bool IsIncludeInActive { get; set; } = false;
    public string? Filter { get; set; } = string.Empty;
}
