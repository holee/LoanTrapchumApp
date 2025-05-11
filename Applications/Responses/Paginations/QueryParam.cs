namespace Applications.Responses.Paginations;

public sealed class QueryParam
{
    public QueryParam()
    {
        PageNumber = 1;
        PageSize = 10;
        SortColumn = string.Empty;
        SortDescending = false;
        IsIncludeInActive = false;
        Filter = string.Empty;
    }
    public QueryParam(int pageNumber, int pageSize, string sortField)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SortColumn = sortField;
    }
    public QueryParam(int pageNumber, int pageSize, string sortField, string? filter = null)
        : this(pageNumber, pageSize, sortField)
    {
        Filter = filter;
    }
    public QueryParam(int pageNumber, int pageSize, string sortField, bool sortDescending = false, string? filter = null)
        : this(pageNumber, pageSize, sortField)
    {
        SortDescending = sortDescending;
        Filter = filter;
    }
    public QueryParam(int pageNumber, int pageSize, string sortField,
        bool sortDescending = false,
        bool isIncludeInActive = false, string? filter = null) : this(pageNumber, pageSize, sortField)
    {
        SortDescending = sortDescending;
        IsIncludeInActive = isIncludeInActive;
        Filter = filter;
    }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; } = "Id";
    public bool SortDescending { get; set; } = false;
    public bool IsIncludeInActive { get; set; } = false;
    public string? Filter { get; set; } = string.Empty;
}
