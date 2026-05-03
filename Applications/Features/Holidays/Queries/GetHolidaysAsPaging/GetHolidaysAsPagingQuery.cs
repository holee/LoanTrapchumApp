using Applications.Features.Holidays.Dtos;

namespace Applications.Features.Holidays.Queries.GetHolidaysAsPaging;

public record GetHolidaysAsPagingQuery(QueryParam Request) : IRequest<Pager<HolidayDto>>
{
}
