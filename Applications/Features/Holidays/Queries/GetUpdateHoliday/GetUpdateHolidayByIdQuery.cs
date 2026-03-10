using Applications.Features.Holidays.Dtos;
using ErrorOr;

namespace Applications.Features.Holidays.Queries.GetUpdateHoliday;

public record GetUpdateHolidayByIdQuery(long? Id) : IRequest<ErrorOr<HolidayDtoUpdate>>; 
