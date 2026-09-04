using Application.Features.Holidays.Dtos;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Holidays.Command.CreateHolidays;

public class CreateHolidaysCommandHandler(IHolidayService holidayService) : IRequestHandler<CreateHolidaysCommand, ErrorOr<IEnumerable<HolidayDto>>>
{
    public async Task<ErrorOr<IEnumerable<HolidayDto>>> Handle(CreateHolidaysCommand request, CancellationToken cancellationToken)
    {

        var req = request.Request.ToHolidays();

        return await holidayService.CreateHolidays(req);

    }
}
