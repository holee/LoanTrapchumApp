using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces.Loans;
using Application.Responses;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.ApprovedLoanRequest;

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
