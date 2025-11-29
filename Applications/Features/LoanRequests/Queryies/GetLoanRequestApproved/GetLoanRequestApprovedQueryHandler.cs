using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestBy;

public sealed class GetLoanRequestApprovedQueryHandler(ILoanRequestService _loanRequestQueryService) : IRequestHandler<GetLoanRequestApprovedQuery, ErrorOr<ApprovedRequest>>
{
    public async Task<ErrorOr<ApprovedRequest>> Handle(GetLoanRequestApprovedQuery request, CancellationToken cancellationToken)
    {
        return await _loanRequestQueryService.GetLoanRequestAsync<ApprovedRequest>(request.BranchId, request.CustomerId, request.LoanRequestId);
    }
}