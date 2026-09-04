using Application.Interfaces.Branches;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchByUser;

public sealed class GetBranchByUserQueryHandler : IRequestHandler<GetBranchByUserQuery, ErrorOr<VBranch>>
{
    private readonly IBranchService _branchService;
    public GetBranchByUserQueryHandler(IBranchService branchService)
    {
        _branchService = branchService;
    }
    public async Task<ErrorOr<VBranch>> Handle(GetBranchByUserQuery request, CancellationToken cancellationToken)
    {
        return await _branchService.GetBranchByUser(request.UserId,cancellationToken);
    }
}