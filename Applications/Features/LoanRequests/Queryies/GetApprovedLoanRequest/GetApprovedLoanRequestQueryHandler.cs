using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;

public sealed class GetApprovedLoanRequestQueryHandler(ILoanRequestService loanRequestRepository)
    : IRequestHandler<GetApprovedLoanRequestQuery, ErrorOr<VLoanRequests>>
{
    public async Task<ErrorOr<VLoanRequests>> Handle(GetApprovedLoanRequestQuery request, CancellationToken cancellationToken)
    {
        return await loanRequestRepository.GetApprovedLoanRequestAsync(
            request.branchId,
            request.customerId,
            request.loanRequestId,
            request.include);

    }
}