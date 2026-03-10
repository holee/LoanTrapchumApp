using Applications.Features.Holidays.Dtos;
using Applications.Features.Holidays.Queries.GetUpdateHoliday;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Holidays.Queries.GetHolidayById;

public sealed class GetUpdateHolidayByIdQueryHandler(IHolidayService holidayService) : IRequestHandler<GetUpdateHolidayByIdQuery, ErrorOr<HolidayDtoUpdate>>
{
    public async Task<ErrorOr<HolidayDtoUpdate>> Handle(GetUpdateHolidayByIdQuery request, CancellationToken cancellationToken = default)
    {
        var rsp = await holidayService.GetHoliday(request.Id, cancellationToken);

        return rsp.Value.ToHolidayUpdate(); 
    }
}
