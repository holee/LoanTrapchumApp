using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.ApplyLoanRequest;

public record AppliedLoanRequestCommand(RequestDtoBaseCreate Request)
        : IRequest<ErrorOr<RequestResult>>;


public sealed class AppliedLoanRequestCommandHandler(ILoanRequestService _loanRequestService)
    : IRequestHandler<AppliedLoanRequestCommand, ErrorOr<RequestResult>>
{
    public async Task<ErrorOr<RequestResult>> Handle(AppliedLoanRequestCommand request, CancellationToken cancellationToken)
    {
        return await _loanRequestService.AppliedLoanRequestAsync(request.Request);
    }
}
