using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchDetailsById;

public class GetBranchDetailsByIdQueryHandler(IBranchService branchService) : IRequestHandler<GetBranchDetailsByIdQuery, ErrorOr<VBranch?>>
{
    public async Task<ErrorOr<VBranch?>> Handle(GetBranchDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        return await branchService.GetBranchDetailsById(request.Id, cancellationToken, request.columns);
    }

}