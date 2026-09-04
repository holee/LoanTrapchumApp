using Application.Features.Dashbaord.Managers.Dto;
using Application.Interfaces;
using ErrorOr;

namespace Application.Features.Dashbaord.Managers.Queries.GetDbManagerLoanSummary;

public sealed class GetDbManagerLoanSummaryQueryHandler(IDashboardService dashboard)
      : IRequestHandler<GetDbManagerLoanSummaryQuery, ErrorOr<DbManagerLoanSummary>>
{
    public async Task<ErrorOr<DbManagerLoanSummary>> Handle(GetDbManagerLoanSummaryQuery request, CancellationToken cancellationToken)
    {
        return await dashboard.GetDbManagerLoanSummary(request.BranchId, request.UserId);
    }
}
