using Application.Features.Holidays.Dtos;
using ErrorOr;

namespace Application.Features.Holidays.Command.CreateHoliday;

public record CreateHolidayCommand(HolidayCreateDto Request) : IRequest<ErrorOr<HolidayDto>>;
