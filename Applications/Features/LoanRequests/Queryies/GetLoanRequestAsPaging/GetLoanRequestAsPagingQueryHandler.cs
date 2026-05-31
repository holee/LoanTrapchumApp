namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestAsPaging;

public class GetLoanRequestAsPagingQueryHandler(ILoanRequestService loanRequestService)
    : IRequestHandler<GetLoanRequestAsPagingQuery, Pager<VLoanRequests>>
{
    public async Task<Pager<VLoanRequests>> Handle(GetLoanRequestAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await loanRequestService.GetLoanRequestsAsync(request.UserId, request.BranchId, request.RequestStatus, request.Param);
    }
}