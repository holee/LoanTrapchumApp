using Applications.Interfaces.Lists;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Holidays.Command.CreateHoliday;

public sealed class CreateHolidayCommandHandler(IHolidayService holidayRepository) : IRequestHandler<CreateHolidayCommand, ErrorOr<long>>
{
    public async Task<ErrorOr<long>> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {
        // Check for existing holiday on the same date

        var req = request.Request.ToHoliday();

        return await holidayRepository.CreateOrUpdate(req);

    }
}