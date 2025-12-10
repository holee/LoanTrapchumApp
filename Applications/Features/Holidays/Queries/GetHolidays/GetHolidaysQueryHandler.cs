using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.Holidays;
using Applications.Utils;

namespace Applications.Features.Holidays.Queries.GetHolidays;

public sealed class GetHolidaysQueryHandler : IRequestHandler<GetHolidaysQuery, IEnumerable<HolidayVm>>
{
    private readonly IHolidayService _holidayRepository;
    public GetHolidaysQueryHandler(IHolidayService holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }
    public async Task<IEnumerable<HolidayVm>> Handle(GetHolidaysQuery request, CancellationToken cancellationToken)
    {
        return await _holidayRepository.GetHolidays(request.dt);
    }
}