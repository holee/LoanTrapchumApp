using Application.Features.Periods.Dto;
using Application.Responses.Paginations;

namespace Application.Features.Periods.Queries.GetPeriods;

public record GetPeriodsQuery(QueryParam? Param) : IRequest<Pager<PeriodDto>>;
