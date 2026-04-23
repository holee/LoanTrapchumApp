using Applications.Features.Periods.Dto;
using ErrorOr;

namespace Applications.Features.Periods.Commands.UpdatePeriod;

public record UpdatePeriodCommand(PeriodDtoUpdate Request) : IRequest<ErrorOr<PeriodDto>>;
