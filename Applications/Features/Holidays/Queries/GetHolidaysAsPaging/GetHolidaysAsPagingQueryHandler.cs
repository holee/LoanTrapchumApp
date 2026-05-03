using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Holidays.Queries.GetHolidaysAsPaging;

public sealed class GetHolidaysAsPagingQueryHandler : IRequestHandler<GetHolidaysAsPagingQuery, Pager<HolidayDto>>
{
    private readonly IHolidayService _holidayRepository;
    public GetHolidaysAsPagingQueryHandler(IHolidayService holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }
    public async Task<Pager<HolidayDto>> Handle(GetHolidaysAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await _holidayRepository.GetHolidays(request.Request, cancellationToken);
    }
}