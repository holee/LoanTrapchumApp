using Applications.Features.Holidays.Dtos;
using ErrorOr;

namespace Applications.Features.Holidays.Command.UpdateHoliday;  

public record UpdateHolidayCommand(HolidayCreateDto Request) : IRequest<ErrorOr<HolidayVm>>; 
