using Application.Features.Periods.Dto;
using ErrorOr;

namespace Application.Features.Periods.Commands.CreatePeriod;

public record CreatePeriodCommand(PeriodDtoCreate Request) : IRequest<ErrorOr<PeriodDto>>;
