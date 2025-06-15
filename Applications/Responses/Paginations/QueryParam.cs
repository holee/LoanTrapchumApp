namespace Applications.Responses.Paginations;

public sealed class QueryParam
{
    public QueryParam()
    {
        Skip = 1;
        PageSize = 10;
        SortColumn = string.Empty;
        SortDescending = false;
        IsIncludeInActive = false;
        Filter = string.Empty;
    }
    public QueryParam(int skip, int pageSize)
    {
        Skip = skip;
        PageSize = pageSize;
    }
    public QueryParam(int skip, int pageSize, string sortField)
        : this(skip, pageSize)
    {
        SortColumn = sortField;
    }
    public QueryParam(int skip, int pageSize, string sortField, string? filter = null)
        : this(skip, pageSize, sortField)
    {
        Filter = filter;
    }

    public QueryParam(int skip, int pageSize, string sortField, bool isIncludeInActive = false)
        : this(skip, pageSize, sortField)
    {
        IsIncludeInActive = isIncludeInActive;
    }
    public QueryParam(int skip, int pageSize, string sortField, bool isIncludeInActive = false, bool sortDescending = false)
        : this(skip, pageSize, sortField, isIncludeInActive)
    {
        SortDescending = sortDescending;
    }
    public QueryParam(int skip, int pageSize, string sortField, bool isIncludeInActive = false, bool sortDescending = false, string? filter = null)
        : this(skip, pageSize, sortField, isIncludeInActive, sortDescending)
    {
        Filter = filter;
    }
    public QueryParam(int skip, int pageSize, string sortField, bool isIncludeInActive = false, bool sortDescending = false, string? filter = null, string? filterColumn = null)
       : this(skip, pageSize, sortField, isIncludeInActive, sortDescending,filter)
    {
        FilterColumn = filterColumn;
    }
    public int Skip { get; set; } = 0;
    public int PageSize { get; set; } = 15;
    public string SortColumn { get; set; } = "Id";
    public bool SortDescending { get; set; } = false;
    public bool IsIncludeInActive { get; set; } = false;
    public string? FilterColumn { get; set; } = string.Empty;
    public string? Filter { get; set; } = string.Empty;
}
