using Application.Features.Periods.Dto;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Periods.Commands.CreatePeriod;

public sealed class CreatePeriodCommandHandler(IPeriodService periodService)
    : IRequestHandler<CreatePeriodCommand, ErrorOr<PeriodDto>>
{
    public Task<ErrorOr<PeriodDto>> Handle(CreatePeriodCommand request, CancellationToken cancellationToken)
        => periodService.CreateAsync(request.Request, cancellationToken);
}
