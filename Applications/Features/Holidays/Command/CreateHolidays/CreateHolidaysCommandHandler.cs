using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Holidays.Command.CreateHolidays;

public class CreateHolidaysCommandHandler(IHolidayService holidayService) : IRequestHandler<CreateHolidaysCommand, ErrorOr<IEnumerable<HolidayVm>>>
{
    public async Task<ErrorOr<IEnumerable<HolidayVm>>> Handle(CreateHolidaysCommand request, CancellationToken cancellationToken)
    {

        var req = request.Request.ToHolidays();

        return await holidayService.CreateHolidays(req);

    }
}
