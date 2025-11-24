using Applications.Interfaces.Loans;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;

public class GetLoanRequestAsPagingQueryHandler(ILoanRequestService loanRequestService)
    : IRequestHandler<GetLoanRequestAsPagingQuery, Pager<VLoanRequests>>
{
    public async Task<Pager<VLoanRequests>> Handle(GetLoanRequestAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await loanRequestService.GetLoanRequestsAsync(request.userId, request.branchId, request.requestStatus, request.param);
    }
}