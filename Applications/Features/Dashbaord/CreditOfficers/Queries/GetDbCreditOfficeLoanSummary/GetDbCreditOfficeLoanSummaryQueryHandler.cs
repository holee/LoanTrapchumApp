using Applications.Features.Dashbaord.CreditOfficers.Dtos;
using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Dashbaord.CreditOfficers.Queries.GetDbCreditOfficeLoanSummary;

public sealed class GetDbCreditOfficeLoanSummaryQueryHandler(IDashboardService dashboardService) : IRequestHandler<GetDbCreditOfficeLoanSummaryQuery, ErrorOr<CoLoanSummary>>
{
    public async Task<ErrorOr<CoLoanSummary>> Handle(GetDbCreditOfficeLoanSummaryQuery request, CancellationToken cancellationToken)
    {
        var rsp= await dashboardService.GetCreditOfficerSummaries(request.BranchId, request.UserId, request.DueDate, cancellationToken);
        
        if(rsp is null)
        {
            return Error.NotFound("CreditOfficerSummary.NotFound", "No credit officer summary found for the given criteria.");
        }

        return rsp;
    }
}
