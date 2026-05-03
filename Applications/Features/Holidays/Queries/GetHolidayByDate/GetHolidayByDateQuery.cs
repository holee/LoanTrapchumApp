using Applications.Features.Holidays.Dtos;
using ErrorOr;

namespace Applications.Features.Holidays.Queries.GetHolidayByDate;

public record GetHolidayByDateQuery(DateTime? Dt) : IRequest<Pager<HolidayDto>>; 
