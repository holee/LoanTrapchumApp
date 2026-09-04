using Application.Features.Dashbaord.Accountants.Dtos;
using Application.Interfaces;
using ErrorOr;

namespace Application.Features.Dashbaord.Accountants.Queries.GetDbAccountLoanSummaery;

public sealed class GetDbAccountLoanSummaryQueryHandler(IDashboardService dashboardService) : IRequestHandler<GetDbAccountLoanSummaryQuery, ErrorOr<AccountSummary>>
{ 
    public async Task<ErrorOr<AccountSummary>> Handle(GetDbAccountLoanSummaryQuery request, CancellationToken cancellationToken)
    {
        return await dashboardService.GetAccountantSummaries(request.BranchId, request.DueDate, cancellationToken);
    }
}
