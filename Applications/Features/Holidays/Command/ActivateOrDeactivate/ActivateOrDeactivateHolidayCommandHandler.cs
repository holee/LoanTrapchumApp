using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Holidays.Command.ActivateOrDeactivate;

public sealed class ActivateOrDeactivateHolidayCommandHandler(IHolidayService holidayRepository) : IRequestHandler<ActivateOrDeactivateHolidayCommand, ErrorOr<HolidayDto>>
{
    public async Task<ErrorOr<HolidayDto>> Handle(ActivateOrDeactivateHolidayCommand request, CancellationToken cancellationToken)
    {
        return await holidayRepository.ActivateOrDeactivate(request.Id, cancellationToken);

    }
}