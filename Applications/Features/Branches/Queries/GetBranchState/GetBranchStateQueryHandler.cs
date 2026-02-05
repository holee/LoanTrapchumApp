using Applications.Interfaces.Branches;
using Applications.Responses;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchState;

public sealed class GetBranchStateQueryHandler(IBranchService branchService) : IRequestHandler<GetBranchStateQuery, ErrorOr<BranchStatus>>
{
    public async Task<ErrorOr<BranchStatus>> Handle(GetBranchStateQuery request, CancellationToken cancellationToken)
    {
        return await branchService.GetBranchStatus(request.UserId, cancellationToken);
    }
}