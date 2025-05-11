using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Requests.Pagigations
{
    public record struct PaginatedResult<T>
    {
        public PaginatedResult(int pageIndex, int pageSize, IEnumerable<T> items, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 15;
        public int PageIndex { get; set; } = 1;
    }
}
