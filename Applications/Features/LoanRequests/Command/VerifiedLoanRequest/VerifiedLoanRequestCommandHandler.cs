using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.VerifiedLoanRequest;

public class VerifiedLoanRequestCommandHandler(ILoanRequestService reqService) : IRequestHandler<VerifiedLoanRequestCommand, ErrorOr<RequestResult>>
{
    public async Task<ErrorOr<RequestResult>> Handle(VerifiedLoanRequestCommand command, CancellationToken cancellationToken)
    {
        return await reqService.VerifiedLoanRequestAsync(command.Request);
    }
}
