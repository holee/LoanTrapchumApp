using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchAsPaging;

public class GetBranchAsPagingQueryHandler(IBranchService branchService) :
            IRequestHandler<GetBranchAsPagingQuery, Pager<VBranch>>
{
    public async Task<Pager<VBranch>> Handle(GetBranchAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await branchService.GetBranchesAsync(request.BranchId, request.Param, cancellationToken);
    }

}