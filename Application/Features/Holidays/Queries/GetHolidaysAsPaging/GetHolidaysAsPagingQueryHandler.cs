using Application.Features.Holidays.Dtos;
using Application.Interfaces.Lists;
using Application.Responses.Paginations;
using Application.Utils;

namespace Application.Features.Holidays.Queries.GetHolidaysAsPaging;

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