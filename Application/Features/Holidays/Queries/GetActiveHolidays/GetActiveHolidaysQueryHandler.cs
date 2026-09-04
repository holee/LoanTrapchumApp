using Application.Features.Holidays.Dtos;
using Application.Interfaces.Lists;

namespace Application.Features.Holidays.Queries.GetActiveHolidays;

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