using Applications.Features.Periods.Dto;
using ErrorOr;

namespace Applications.Features.Periods.Commands.CreatePeriod;

public record CreatePeriodCommand(PeriodDtoCreate Request) : IRequest<ErrorOr<PeriodDto>>;
