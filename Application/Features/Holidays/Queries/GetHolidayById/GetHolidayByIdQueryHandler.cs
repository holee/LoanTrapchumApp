using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Holidays.Queries.GetHolidayById;

public sealed class GetHolidayByIdQueryHandler(IHolidayService holidayService) : IRequestHandler<GetHolidayByIdQuery, ErrorOr<Holiday>>{
    public Task<ErrorOr<Holiday>> Handle(GetHolidayByIdQuery request, CancellationToken cancellationToken = default)
    {
        return holidayService.GetHoliday(request.Id, cancellationToken);
    }
}
