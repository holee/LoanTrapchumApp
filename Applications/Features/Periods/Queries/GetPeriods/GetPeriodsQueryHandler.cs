using Applications.Features.Periods.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;

namespace Applications.Features.Periods.Queries.GetPeriods;

public sealed class GetPeriodsQueryHandler(IPeriodService periodService)
    : IRequestHandler<GetPeriodsQuery, Pager<PeriodDto>>
{
    public Task<Pager<PeriodDto>> Handle(GetPeriodsQuery request, CancellationToken cancellationToken)
        => periodService.GetAllAsync(request.Param, cancellationToken);
}
