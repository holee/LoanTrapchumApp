using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Dashbaord.Accountants.Queries.GetDbApprovedLoanRequest;

public sealed class GetDbApprovedRequestHandler(IDashboardService dashboardService) : IRequestHandler<GetDbApprovedRequestQuery, ErrorOr<int>>
{
    public async Task<ErrorOr<int>> Handle(GetDbApprovedRequestQuery request, CancellationToken cancellationToken)
    {
        return await dashboardService.GetApprovedLoanRequest(request.BranchId, cancellationToken);
    }
}
