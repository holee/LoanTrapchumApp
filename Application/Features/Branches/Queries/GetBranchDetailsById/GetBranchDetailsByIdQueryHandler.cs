using Application.Features.Branches.Dtos;
using Application.Interfaces.Branches;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchDetailsById;

public class GetBranchDetailsByIdQueryHandler(IBranchService branchService) : IRequestHandler<GetBranchDetailsByIdQuery, ErrorOr<VBranch?>>
{
    public async Task<ErrorOr<VBranch?>> Handle(GetBranchDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        return await branchService.GetBranchDetailsById(request.Id, cancellationToken, request.columns);
    }

}