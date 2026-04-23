using Applications.Features.Periods.Dto;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Periods.Commands.UpdatePeriod;

public sealed class UpdatePeriodCommandHandler(IPeriodService periodService)
    : IRequestHandler<UpdatePeriodCommand, ErrorOr<PeriodDto>>
{
    public Task<ErrorOr<PeriodDto>> Handle(UpdatePeriodCommand request, CancellationToken cancellationToken)
        => periodService.UpdateAsync(request.Request, cancellationToken);
}
