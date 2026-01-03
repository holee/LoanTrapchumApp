using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestDetailById;

public sealed class GetLoanRequestDetailByIdQueryHandler : IRequestHandler<GetLoanRequestDetailByIdQuery, ErrorOr<VLoanRequests>>
{
    private readonly ILoanRequestService _loanRequestService;

    public GetLoanRequestDetailByIdQueryHandler(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }

    public async Task<ErrorOr<VLoanRequests>> Handle(GetLoanRequestDetailByIdQuery request, CancellationToken cancellationToken)
    {
        return await _loanRequestService.GetLoanRequestAsync(request.BranchId, request.CustomerId, request.LoanRequestId, request.Include);
    }
}