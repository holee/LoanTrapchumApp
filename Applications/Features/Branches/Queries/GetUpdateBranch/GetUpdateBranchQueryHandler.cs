using Applications.Features.Branches.Dtos;
using Applications.Interfaces.Branches;
using ErrorOr;
using Mapster;

namespace Applications.Features.Branches.Queries.GetUpdateBranch;

public class GetUpdateBranchQueryHandler(IBranchService branchService) : IRequestHandler<GetUpdateBranchQuery, ErrorOr<BranchDtoUpdate>>
{
    public async Task<ErrorOr<BranchDtoUpdate>> Handle(GetUpdateBranchQuery request, CancellationToken cancellationToken)
    {

        var model = await branchService.GetBranchById(request.BranchId, cancellationToken);

        var result = model.Value.Adapt<BranchDtoUpdate>();

        return result;
    }

}