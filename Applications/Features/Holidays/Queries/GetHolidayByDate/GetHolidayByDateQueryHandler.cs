using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;

namespace Applications.Features.Holidays.Queries.GetHolidayByDate;

public sealed class GetHolidayByDateQueryHandler(IHolidayService holidayService) : IRequestHandler<GetHolidayByDateQuery, IEnumerable<HolidayDto>>
{
    public async Task<IEnumerable<HolidayDto>> Handle(GetHolidayByDateQuery request, CancellationToken cancellationToken = default)
    {
        var holidays = await holidayService.GetHolidays(new QueryParam(0, 0), request.Dt, cancellationToken);

        return holidays.Items;
    }
}
