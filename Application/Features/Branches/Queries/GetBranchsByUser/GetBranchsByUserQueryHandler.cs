using Application.Interfaces.Branches;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchsByUser;

public sealed class GetBranchsByUserQueryHandler : IRequestHandler<GetBranchsByUserQuery, ErrorOr<IEnumerable<VBranch>>>
{
    private readonly IBranchService _branchService;
    public GetBranchsByUserQueryHandler(IBranchService branchService)
    {
        _branchService = branchService;
    }
    public async Task<ErrorOr<IEnumerable<VBranch>>> Handle(GetBranchsByUserQuery request, CancellationToken cancellationToken)
    {
        return await _branchService.GetBranchesByUser(request.UserId, request.BranchId);
    }
}