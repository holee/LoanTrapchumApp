using Application.Features.Holidays.Dtos;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Holidays.Command.CreateHoliday;

public sealed class CreateHolidayCommandHandler(IHolidayService holidayRepository) : IRequestHandler<CreateHolidayCommand, ErrorOr<HolidayDto>>
{
    public async Task<ErrorOr<HolidayDto>> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {

        var req = request.Request.ToHoliday();

        return await holidayRepository.CreateOrUpdate(req, cancellationToken);

    }
}