using Application.Features.Holidays.Dtos;

namespace Application.Features.Holidays.Queries.GetHolidaysAsPaging;

public record GetHolidaysAsPagingQuery(QueryParam Request) : IRequest<Pager<HolidayDto>>
{
}
