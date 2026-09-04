using Application.Features.Branches.Dtos;
using Application.Interfaces.Branches;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchForVault;

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
