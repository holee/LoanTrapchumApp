using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.VerifiedLoanRequest;

public class VerifiedLoanRequestCommandHandler(ILoanRequestService reqService) : IRequestHandler<VerifiedLoanRequestCommand, ErrorOr<RequestResult>>
{
    public async Task<ErrorOr<RequestResult>> Handle(VerifiedLoanRequestCommand command, CancellationToken cancellationToken)
    {
        return await reqService.VerifiedLoanRequestAsync(command.Request);
    }
}
