using Application.Features.Periods.Dto;
using Application.Interfaces.Lists;
using Application.Responses.Paginations;

namespace Application.Features.Periods.Queries.GetPeriods;

public sealed class GetPeriodsQueryHandler(IPeriodService periodService)
    : IRequestHandler<GetPeriodsQuery, Pager<PeriodDto>>
{
    public Task<Pager<PeriodDto>> Handle(GetPeriodsQuery request, CancellationToken cancellationToken)
        => periodService.GetAllAsync(request.Param, cancellationToken);
}
