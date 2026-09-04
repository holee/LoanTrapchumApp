using Application.Features.Holidays.Dtos;
using ErrorOr;

namespace Application.Features.Holidays.Queries.GetHolidayByDate;

public record GetHolidayByDateQuery(DateTime? Dt) : IRequest<Pager<HolidayDto>>; 
