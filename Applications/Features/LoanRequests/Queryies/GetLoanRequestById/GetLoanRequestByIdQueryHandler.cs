using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestById;

public sealed class GetLoanRequestByIdQueryHandler : IRequestHandler<GetLoanRequestByIdQuery, ErrorOr<VLoanRequests>>
{
    private readonly ILoanRequestService _loanRequestService;

    public GetLoanRequestByIdQueryHandler(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }

    public async Task<ErrorOr<VLoanRequests>> Handle(GetLoanRequestByIdQuery request, CancellationToken cancellationToken)
    {
        return await _loanRequestService.GetLoanRequestAsync(request.BranchId, request.CustomerId, request.LoanRequestId, request.Include);
    }
}
