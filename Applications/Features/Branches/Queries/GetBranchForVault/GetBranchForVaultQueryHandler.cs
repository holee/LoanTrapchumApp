using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchForVault;

public sealed class GetBranchForVaultQueryHandler : IRequestHandler<GetBranchForVaultQuery, ErrorOr<VBranch>>
{
    private readonly IBranchService _branchService;
    public GetBranchForVaultQueryHandler(IBranchService branchService)
    {
        _branchService = branchService;
    }
    public async Task<ErrorOr<VBranch>> Handle(GetBranchForVaultQuery request, CancellationToken cancellationToken)
    {
        return await _branchService.GetBranchForVault(request.BranchId);

    }
}
