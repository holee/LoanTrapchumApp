using Applications.Interfaces.Branches;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchById;

public class GetBranchByIdQueryHandler(IBranchService branchService) : IRequestHandler<GetBranchByIdQuery, ErrorOr<Branch>>
{
    public async Task<ErrorOr<Branch>> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        return await branchService.GetBranchById(request.Id, cancellationToken);
    }

}