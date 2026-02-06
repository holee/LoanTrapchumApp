using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using ErrorOr;
using Mapster;

namespace Applications.Features.Branches.Command.ChangeBranchState;

public sealed class ChangeBranchStateCommandHandler(IBranchService branchService) : IRequestHandler<ChangeBranchStateCommand, ErrorOr<VBranch>>
{
    public async Task<ErrorOr<VBranch>> Handle(ChangeBranchStateCommand request, CancellationToken cancellationToken = default)
    {
        return await branchService.UpdateStatusAsync(request.BranchId, request.State, cancellationToken);
    }
}
