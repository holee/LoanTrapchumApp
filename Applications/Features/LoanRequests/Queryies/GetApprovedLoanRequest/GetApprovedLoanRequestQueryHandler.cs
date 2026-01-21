using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetApprovedLoanRequest;

public sealed class GetApprovedLoanRequestQueryHandler(ILoanRequestService loanRequestRepository)
    : IRequestHandler<GetApprovedLoanRequestQuery, ErrorOr<VLoanRequests>>
{
    public async Task<ErrorOr<VLoanRequests>> Handle(GetApprovedLoanRequestQuery request, CancellationToken cancellationToken)
    {
        return await loanRequestRepository.GetApprovedLoanRequestAsync(
            request.BranchId,
            request.CustomerId,
            request.LoanRequestId,
            request.Include);

    }
}