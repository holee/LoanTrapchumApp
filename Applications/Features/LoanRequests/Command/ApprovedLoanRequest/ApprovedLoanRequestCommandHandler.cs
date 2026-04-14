using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.ApprovedLoanRequest;

public sealed class ApprovedLoanRequestCommandHandler : IRequestHandler<ApprovedRequestCommand, ErrorOr<RequestResult>>
{

    private readonly ILoanRequestService _loanRequestService;

    public ApprovedLoanRequestCommandHandler(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }
    public async Task<ErrorOr<RequestResult>> Handle(ApprovedRequestCommand request, CancellationToken cancellationToken)
    {                   
        return await _loanRequestService.ApprovedLoanRequestAsync(request.Request);
    }
}
