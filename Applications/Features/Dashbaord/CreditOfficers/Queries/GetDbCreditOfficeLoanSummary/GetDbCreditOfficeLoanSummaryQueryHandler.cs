using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Interfaces;

namespace Applications.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;

public sealed class GetDbCreditOfficeLoanSummaryQueryHandler(IDashboardService dashboardService) : IRequestHandler<GetDbCreditOfficeLoanSummaryQuery, Pager<DbCoLoanSummary>>
{
    public async Task<Pager<DbCoLoanSummary>> Handle(GetDbCreditOfficeLoanSummaryQuery request, CancellationToken cancellationToken)
    {
        return await dashboardService.GetDbCreditOfficerSummaries(request.BranchId, request.UserId, request.DueDate, cancellationToken);
    }
}
