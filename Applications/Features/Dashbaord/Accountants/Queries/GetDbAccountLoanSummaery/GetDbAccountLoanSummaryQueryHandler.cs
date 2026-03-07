using Applications.Features.Dashbaord.Accountants.Dtos;
using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;

public sealed class GetDbAccountLoanSummaryQueryHandler(IDashboardService dashboardService) : IRequestHandler<GetDbAccountLoanSummaryQuery, Pager<AccountantLoanSummary>>
{ 
    public async Task<Pager<AccountantLoanSummary>> Handle(GetDbAccountLoanSummaryQuery request, CancellationToken cancellationToken)
    {
        return await dashboardService.GetAccountantLoanSummaries(request.BranchId, request.DueDate, cancellationToken);
    }
}
