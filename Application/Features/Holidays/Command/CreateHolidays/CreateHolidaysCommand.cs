using Application.Features.Holidays.Dtos;
using ErrorOr;

namespace Application.Features.Holidays.Command.CreateHolidays;

public record CreateHolidaysCommand(List<HolidayCreateDto> Request) : IRequest<ErrorOr<IEnumerable<HolidayDto>>>;
