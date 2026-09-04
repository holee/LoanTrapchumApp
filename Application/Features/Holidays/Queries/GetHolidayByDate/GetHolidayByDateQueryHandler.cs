using Application.Features.Holidays.Dtos;
using Application.Interfaces.Lists;

namespace Application.Features.Holidays.Queries.GetHolidayByDate;

public sealed class GetHolidayByDateQueryHandler(IHolidayService holidayService) : IRequestHandler<GetHolidayByDateQuery, IEnumerable<HolidayDto>>
{
    public async Task<IEnumerable<HolidayDto>> Handle(GetHolidayByDateQuery request, CancellationToken cancellationToken = default)
    {
        var holidays = await holidayService.GetHolidays(new QueryParam(0, 0), request.Dt, cancellationToken);

        return holidays.Items;
    }
}
