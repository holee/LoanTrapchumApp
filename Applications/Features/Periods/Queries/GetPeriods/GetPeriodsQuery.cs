using Applications.Features.Periods.Dto;
using Applications.Responses.Paginations;

namespace Applications.Features.Periods.Queries.GetPeriods;

public record GetPeriodsQuery(QueryParam? Param) : IRequest<Pager<PeriodDto>>;
