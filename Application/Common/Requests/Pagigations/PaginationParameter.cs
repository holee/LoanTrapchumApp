using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests.Pagigations
{
    public sealed class PaginationParameter
    {
        public PaginationParameter()
        {

            PageNumber = 1;
            PageSize = 15;
            SortColumn = string.Empty;
            SortDescending = false;
            IncludeInactive = false;
        }
        public PaginationParameter(int pageNumber, int pageSize, string? sortColumn, bool sortDescending = false, bool? includeInactive = false, string? filter = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortColumn = sortColumn;
            SortDescending = sortDescending;
            IncludeInactive = includeInactive;
            Filter = filter;
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public string? SortColumn { get; set; } = string.Empty;
        public bool SortDescending { get; set; } = false;
        public bool? IncludeInactive { get; set; } = false;
        public string? Filter { get; set; } = string.Empty;

        public int Skip => (PageNumber - 1) * PageSize;
    }
}
