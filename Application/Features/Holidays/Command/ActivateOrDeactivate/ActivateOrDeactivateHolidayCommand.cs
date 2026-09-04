using Application.Features.Holidays.Dtos;
using ErrorOr;

namespace Application.Features.Holidays.Command.ActivateOrDeactivate; 

public record ActivateOrDeactivateHolidayCommand(long? Id) : IRequest<ErrorOr<HolidayDto>>; 
