using Applications.Features.Holidays.Dtos;
using ErrorOr;

namespace Applications.Features.Holidays.Command.CreateHoliday;

public record CreateHolidayCommand(HolidayCreateDto Request) : IRequest<ErrorOr<HolidayDto>>;
