namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestsWithCanDisburse;

public sealed class GetLoanRequestsWithCanDisburseQueryHandler(ILoanRequestService loanRequestRepository)
    : IRequestHandler<GetLoanRequestsWithCanDisburseQuery, Pager<VLoanRequests>>
{
    public async Task<Pager<VLoanRequests>> Handle(GetLoanRequestsWithCanDisburseQuery request, CancellationToken cancellationToken)
    {
        return await loanRequestRepository.GetLoanRequestsAsync(request.UserId, request.Candisburse, request.BranchId, request.RequestStatus, request.Param);
    }
}