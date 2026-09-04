using Application.Features.Holidays.Dtos;
using Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Holidays.Queries.GetHolidays;

public record GetHolidaysQuery(DateTime dt) : IRequest<IEnumerable<HolidayDto>>
{
}
