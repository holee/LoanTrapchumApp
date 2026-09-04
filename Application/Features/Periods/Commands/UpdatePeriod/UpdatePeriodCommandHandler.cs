using Application.Features.Periods.Dto;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Periods.Commands.UpdatePeriod;

public sealed class UpdatePeriodCommandHandler(IPeriodService periodService)
    : IRequestHandler<UpdatePeriodCommand, ErrorOr<PeriodDto>>
{
    public Task<ErrorOr<PeriodDto>> Handle(UpdatePeriodCommand request, CancellationToken cancellationToken)
        => periodService.UpdateAsync(request.Request, cancellationToken);
}
