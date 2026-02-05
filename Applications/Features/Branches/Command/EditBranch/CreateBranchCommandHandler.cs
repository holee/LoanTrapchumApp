using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using ErrorOr;
using Mapster;

namespace Applications.Features.Branches.Command.EditBranch;

public sealed class EditBranchCommandHandler(IBranchService branchService) : IRequestHandler<EditBranchCommand, ErrorOr<VBranch>>
{
    public async Task<ErrorOr<VBranch>> Handle(EditBranchCommand request, CancellationToken cancellationToken = default)
    {
        var branch = request.Branch.Adapt<Branch>();

        return await branchService.UpdateAsync(branch, cancellationToken);
    }
}
