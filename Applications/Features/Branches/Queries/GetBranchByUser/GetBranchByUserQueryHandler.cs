using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchByUser;

public sealed class GetBranchByUserQueryHandler : IRequestHandler<GetBranchByUserQuery, ErrorOr<IEnumerable<VBranch>>>
{
    private readonly IBranchService _branchService;
    public GetBranchByUserQueryHandler(IBranchService branchService)
    {
        _branchService = branchService;
    }
    public async Task<ErrorOr<IEnumerable<VBranch>>> Handle(GetBranchByUserQuery request, CancellationToken cancellationToken)
    {
        return await _branchService.GetBranchesByUser(request.UserId, request.BranchId);
    }
}