namespace Applications.Features.Holidays.Queries.GetHolidaysAsPaging;

public record GetHolidaysAsPagingQuery(QueryParam Request) : IRequest<Pager<HolidayVm>>
{
}
