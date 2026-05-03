using Applications.Features.Holidays.Dtos;
using Applications.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Holidays.Queries.GetHolidays;

public record GetHolidaysQuery(DateTime dt) : IRequest<IEnumerable<HolidayDto>>
{
}
