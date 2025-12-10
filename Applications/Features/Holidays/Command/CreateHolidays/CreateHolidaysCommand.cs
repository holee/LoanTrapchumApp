using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.Holidays;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Holidays.Command.CreateHolidays;

public record CreateHolidaysCommand(List<HolidayCreateDto> Request) : IRequest<ErrorOr<IEnumerable<HolidayVm>>>;

public class CreateHolidaysCommandHandler(IHolidayService holidayService) : IRequestHandler<CreateHolidaysCommand, ErrorOr<IEnumerable<HolidayVm>>>
{
    public async Task<ErrorOr<IEnumerable<HolidayVm>>> Handle(CreateHolidaysCommand request, CancellationToken cancellationToken)
    {

        var req = request.Request.ToHolidays();

        return await holidayService.CreateHolidays(req);

    }
}
