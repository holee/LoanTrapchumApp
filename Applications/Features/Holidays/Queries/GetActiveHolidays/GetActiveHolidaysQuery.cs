using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Holidays.Queries.GetActiveHolidays;

public record GetActiveHolidaysQuery(int? EndYear = 1) : IRequest<ErrorOr<Pager<HolidayVm>>>;

public sealed class GetActiveHolidaysQueryHandler : IRequestHandler<GetActiveHolidaysQuery, ErrorOr<Pager<HolidayVm>>>
{
    private readonly IHolidayService _holidayService;
    public GetActiveHolidaysQueryHandler(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }
    public async Task<ErrorOr<Pager<HolidayVm>>> Handle(GetActiveHolidaysQuery request, CancellationToken cancellationToken)
    {
        return await _holidayService.GetActiveHolidays(request.EndYear, cancellationToken);
    }
}