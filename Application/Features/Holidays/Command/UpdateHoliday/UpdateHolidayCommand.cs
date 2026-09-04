using Application.Features.Holidays.Dtos;
using ErrorOr;

namespace Application.Features.Holidays.Command.UpdateHoliday;  

public record UpdateHolidayCommand(HolidayCreateDto Request) : IRequest<ErrorOr<HolidayDto>>; 
