using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;

namespace Applications.Features.Holidays.Queries.GetActiveHolidays;

public sealed class GetActiveHolidaysQueryHandler : IRequestHandler<GetActiveHolidaysQuery, Pager<HolidayDto>>
{
    private readonly IHolidayService _holidayService;
    public GetActiveHolidaysQueryHandler(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }
    public async Task<Pager<HolidayDto>> Handle(GetActiveHolidaysQuery request, CancellationToken cancellationToken)
    {
        var holidays = await _holidayService.GetActiveHolidays(request.EndYear, cancellationToken);
        return holidays;
    }
}