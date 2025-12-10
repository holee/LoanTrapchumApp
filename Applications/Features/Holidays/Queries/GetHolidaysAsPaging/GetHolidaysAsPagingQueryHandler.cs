using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Holidays;
using Applications.Utils;

namespace Applications.Features.Holidays.Queries.GetHolidaysAsPaging;

public sealed class GetHolidaysAsPagingQueryHandler : IRequestHandler<GetHolidaysAsPagingQuery, Pager<HolidayVm>>
{
    private readonly IHolidayService _holidayRepository;
    public GetHolidaysAsPagingQueryHandler(IHolidayService holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }
    public async Task<Pager<HolidayVm>> Handle(GetHolidaysAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await _holidayRepository.GetHolidays(request.Request, cancellationToken);
    }
}