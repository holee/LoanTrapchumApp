using Applications.Interfaces.Branches;
using ErrorOr;
using Mapster;

namespace Applications.Features.Branches.Command.CreateBranch;

public sealed class CreateBranchCommandHandler(IBranchService branchService) : IRequestHandler<CreateBranchCommand, ErrorOr<VBranch>>
{
    public async Task<ErrorOr<VBranch>> Handle(CreateBranchCommand request, CancellationToken cancellationToken = default)
    {
        var branch = request.Branch.Adapt<Branch>();

        return await branchService.CreateAsync(branch, cancellationToken);
    }
}
