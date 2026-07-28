namespace Applications.Responses.Paginations;

public sealed class QueryParam
{
    public QueryParam()
    {
        Page = 0;
        Take = 0;
        SortColumn = string.Empty;
        SortDescending = false;
        IncludeInActive = false;
        Filter = string.Empty;
    }
    public QueryParam(int page, int pageSize)
    {
        Page = page;
        Take = pageSize;
    }
    public QueryParam(int page, int pageSize, string? sortField)
        : this(page, pageSize)
    {
        SortColumn = sortField;
    }
    public QueryParam(int page, int pageSize, string? sortField, string? filter = null)
        : this(page, pageSize, sortField)
    {
        Filter = filter;
    }

    public QueryParam(int page, int pageSize, string? sortField, bool isIncludeInActive = false)
        : this(page, pageSize, sortField)
    {
        IncludeInActive = isIncludeInActive;
    }
    public QueryParam(int page, int pageSize, string? sortField, bool isIncludeInActive = false, bool sortDescending = false)
        : this(page, pageSize, sortField, isIncludeInActive)
    {
        SortDescending = sortDescending;
    }
    public QueryParam(int page,
                      int pageSize,
                      string? sortField,
                      bool isIncludeInActive = false,
                      bool sortDescending = false,
                      string? filter = null)
        : this(page, pageSize, sortField, isIncludeInActive, sortDescending)
    {
        Filter = filter;
    }


    public QueryParam(int page,
                      int take,
                      string? filter = null,
                      string? sortField = null,
                      string? filterColumn = null,
                      bool isIncludeInActive = false,
                      bool sortDescending = false)
       : this(page, take, sortField, isIncludeInActive, sortDescending)
    {
        Filter = filter;
        FilterColumn = filterColumn;
    }


    public QueryParam(int page,
                      int take,
                      string sortField,
                      bool isIncludeInActive = false,
                      bool sortDescending = false,
                      string? filter = null,
                      string? filterColumn = null)
       : this(page, take, sortField, isIncludeInActive, sortDescending, filter)
    {
        FilterColumn = filterColumn;
    }



    public int Page { get; set; } = 0;
    public int Take { get; set; } = 15;
    public string? SortColumn { get; set; } = string.Empty;
    public bool SortDescending { get; set; } = false;
    public bool IncludeInActive { get; set; } = false;
    public string? FilterColumn { get; set; } = string.Empty;
    public string[] SelectColumns { get; set; } = [];
    public string? Filter { get; set; } = string.Empty;
}
