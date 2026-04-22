using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Periods.Commands.DeletePeriod;

public sealed class DeletePeriodCommandHandler(IPeriodService periodService)
    : IRequestHandler<DeletePeriodCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(DeletePeriodCommand request, CancellationToken cancellationToken)
        => periodService.DeleteAsync(request.Id, cancellationToken);
}
