using Application.Features.Holidays.Dtos;
using Application.Interfaces.Lists;

namespace Application.Features.Holidays.Queries.GetHolidays;

public sealed class GetHolidaysQueryHandler : IRequestHandler<GetHolidaysQuery, IEnumerable<HolidayDto>>
{
    private readonly IHolidayService _holidayRepository;
    public GetHolidaysQueryHandler(IHolidayService holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }
    public async Task<IEnumerable<HolidayDto>> Handle(GetHolidaysQuery request, CancellationToken cancellationToken)
    {
        return await _holidayRepository.GetHolidays(request.dt);
    }
}