using Application.Interfaces.Lists;
using Application.Responses.ViewModels.OtherNames;
using ErrorOr;

namespace Application.Features.OtherNames.Commands.UpdateOtherNameStatus;

public sealed class UpdateOtherNameStatusCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<UpdateOtherNameStatusCommand, ErrorOr<VOtherNames>>
{
    public async Task<ErrorOr<VOtherNames>> Handle(UpdateOtherNameStatusCommand request, CancellationToken cancellationToken)
    {
        return await otherNameService.UpdateStatusAsync(request.Id, request.State, cancellationToken);
    }
}
