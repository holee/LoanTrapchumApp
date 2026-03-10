using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Holidays.Command.CreateHoliday;

public sealed class CreateHolidayCommandHandler(IHolidayService holidayRepository) : IRequestHandler<CreateHolidayCommand, ErrorOr<HolidayVm>>
{
    public async Task<ErrorOr<HolidayVm>> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {

        var req = request.Request.ToHoliday();

        return await holidayRepository.CreateOrUpdate(req, cancellationToken);

    }
}