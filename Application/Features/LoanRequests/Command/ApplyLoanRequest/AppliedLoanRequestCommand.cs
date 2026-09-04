using Application.Features.LoanRequests.Requests;
using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.ApplyLoanRequest;

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
