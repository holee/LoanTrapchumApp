using Applications.Interfaces.Lists;

namespace Applications.Features.Holidays.Queries.GetActiveHolidays;

public sealed class GetActiveHolidaysQueryHandler : IRequestHandler<GetActiveHolidaysQuery, Pager<HolidayVm>>
{
    private readonly IHolidayService _holidayService;
    public GetActiveHolidaysQueryHandler(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }
    public async Task<Pager<HolidayVm>> Handle(GetActiveHolidaysQuery request, CancellationToken cancellationToken)
    {
        var holidays = await _holidayService.GetActiveHolidays(request.EndYear, cancellationToken);
        return holidays;
    }
}