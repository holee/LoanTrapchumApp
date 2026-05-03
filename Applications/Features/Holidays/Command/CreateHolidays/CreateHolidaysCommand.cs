using Applications.Features.Holidays.Dtos;
using ErrorOr;

namespace Applications.Features.Holidays.Command.CreateHolidays;

public record CreateHolidaysCommand(List<HolidayCreateDto> Request) : IRequest<ErrorOr<IEnumerable<HolidayDto>>>;
