using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Holidays.Command.ActivateOrDeactivate;

public sealed class ActivateOrDeactivateHolidayCommandHandler(IHolidayService holidayRepository) : IRequestHandler<ActivateOrDeactivateHolidayCommand, ErrorOr<HolidayVm>>
{
    public async Task<ErrorOr<HolidayVm>> Handle(ActivateOrDeactivateHolidayCommand request, CancellationToken cancellationToken)
    {
        return await holidayRepository.ActivateOrDeactivate(request.Id, cancellationToken);

    }
}