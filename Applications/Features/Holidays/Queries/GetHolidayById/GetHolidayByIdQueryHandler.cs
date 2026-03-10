using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Holidays.Queries.GetHolidayById;

public sealed class GetHolidayByIdQueryHandler(IHolidayService holidayService) : IRequestHandler<GetHolidayByIdQuery, ErrorOr<Holiday>>{
    public Task<ErrorOr<Holiday>> Handle(GetHolidayByIdQuery request, CancellationToken cancellationToken = default)
    {
        return holidayService.GetHoliday(request.Id, cancellationToken);
    }
}
