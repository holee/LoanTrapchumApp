using Applications.Commons.Mappers;
using Applications.Interfaces.Branches;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetUpdateBranch;

public class GetUpdateBranchQueryHandler(IBranchService branchService) : IRequestHandler<GetUpdateBranchQuery, ErrorOr<BranchDtoUpdate>>
{
    public async Task<ErrorOr<BranchDtoUpdate>> Handle(GetUpdateBranchQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rsp = await branchService.GetBranchById(request.BranchId, cancellationToken);

            if (rsp.IsError)
            {
                return rsp.Errors;
            }

            return rsp.Value.ToBranchDtoUpdate();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing the request.", ex);
        }
    }

}