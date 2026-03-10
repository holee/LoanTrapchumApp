using Applications.Features.Holidays.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Holidays.Command.UpdateHoliday;

public class UpdateHolidayCommandHandler(IHolidayService holidayService) : IRequestHandler<UpdateHolidayCommand, ErrorOr<HolidayVm>>
{
    public async Task<ErrorOr<HolidayVm>> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
    {

        var req = request.Request.ToHoliday(); 

        return await holidayService.CreateOrUpdate(req); 

    }
}
