using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Applications.Features.OtherNames.Commands.UpdateOtherNameStatus;

public sealed class UpdateOtherNameStatusCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<UpdateOtherNameStatusCommand, ErrorOr<VOtherNames>>
{
    public async Task<ErrorOr<VOtherNames>> Handle(UpdateOtherNameStatusCommand request, CancellationToken cancellationToken)
    {
        return await otherNameService.UpdateStatusAsync(request.Id, request.State, cancellationToken);
    }
}
