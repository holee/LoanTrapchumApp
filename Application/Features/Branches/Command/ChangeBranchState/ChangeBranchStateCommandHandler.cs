using Application.Features.Branches.Dtos;
using Application.Interfaces.Branches;
using ErrorOr;
using Mapster;

namespace Application.Features.Branches.Command.ChangeBranchState;

public sealed class ChangeBranchStateCommandHandler(IBranchService branchService) : IRequestHandler<ChangeBranchStateCommand, ErrorOr<VBranch>>
{
    public async Task<ErrorOr<VBranch>> Handle(ChangeBranchStateCommand request, CancellationToken cancellationToken = default)
    {
        return await branchService.UpdateStatusAsync(request.BranchId, request.State, cancellationToken);
    }
}
