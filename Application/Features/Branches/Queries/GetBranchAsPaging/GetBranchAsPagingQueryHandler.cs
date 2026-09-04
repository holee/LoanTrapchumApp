using Application.Features.Branches.Dtos;
using Application.Interfaces.Branches;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchAsPaging;

public class GetBranchAsPagingQueryHandler(IBranchService branchService) :
            IRequestHandler<GetBranchAsPagingQuery, Pager<VBranch>>
{
    public async Task<Pager<VBranch>> Handle(GetBranchAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await branchService.GetBranchesAsync(request.BranchId, request.Param, cancellationToken);
    }

}