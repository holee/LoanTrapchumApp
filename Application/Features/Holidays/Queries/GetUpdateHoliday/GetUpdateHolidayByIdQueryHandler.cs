using Application.Features.Holidays.Dtos;
using Application.Features.Holidays.Queries.GetUpdateHoliday;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Holidays.Queries.GetHolidayById;

public sealed class GetUpdateHolidayByIdQueryHandler(IHolidayService holidayService) : IRequestHandler<GetUpdateHolidayByIdQuery, ErrorOr<HolidayDtoUpdate>>
{
    public async Task<ErrorOr<HolidayDtoUpdate>> Handle(GetUpdateHolidayByIdQuery request, CancellationToken cancellationToken = default)
    {
        var rsp = await holidayService.GetHoliday(request.Id, cancellationToken);

        return rsp.Value.ToHolidayUpdate(); 
    }
}
