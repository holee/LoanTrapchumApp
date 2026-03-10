using Applications.Features.Holidays.Dtos;
using ErrorOr;

namespace Applications.Features.Holidays.Command.ActivateOrDeactivate; 

public record ActivateOrDeactivateHolidayCommand(long? Id) : IRequest<ErrorOr<HolidayVm>>; 
