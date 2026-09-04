using Application.Features.Periods.Dto;
using ErrorOr;

namespace Application.Features.Periods.Commands.UpdatePeriod;

public record UpdatePeriodCommand(PeriodDtoUpdate Request) : IRequest<ErrorOr<PeriodDto>>;
