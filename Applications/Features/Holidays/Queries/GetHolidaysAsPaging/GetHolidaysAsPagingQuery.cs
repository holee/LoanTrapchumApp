using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Holidays;
using Applications.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Holidays.Queries.GetHolidaysAsPaging;

public record GetHolidaysAsPagingQuery(QueryParam Request) : IRequest<Pager<HolidayVm>>
{
}
