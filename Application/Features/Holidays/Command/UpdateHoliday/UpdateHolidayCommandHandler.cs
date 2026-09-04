using Application.Features.Holidays.Dtos;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Holidays.Command.UpdateHoliday;

public class UpdateHolidayCommandHandler(IHolidayService holidayService) : IRequestHandler<UpdateHolidayCommand, ErrorOr<HolidayDto>>
{
    public async Task<ErrorOr<HolidayDto>> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
    {

        var req = request.Request.ToHoliday(); 

        return await holidayService.CreateOrUpdate(req); 

    }
}
