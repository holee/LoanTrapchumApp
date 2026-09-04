using Application.Features.Holidays.Dtos;
using ErrorOr;

namespace Application.Features.Holidays.Queries.GetUpdateHoliday;

public record GetUpdateHolidayByIdQuery(long? Id) : IRequest<ErrorOr<HolidayDtoUpdate>>; 
