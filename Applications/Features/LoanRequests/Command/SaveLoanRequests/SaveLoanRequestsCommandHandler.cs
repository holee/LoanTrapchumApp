using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.SaveLoanRequests; 

public class SaveLoanRequestsCommandHandler : IRequestHandler<SaveLoanRequestsCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _loanService;
    public SaveLoanRequestsCommandHandler(ILoanRequestService loanService) 
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<RequestResult>> Handle(SaveLoanRequestsCommand request, CancellationToken cancellationToken)
    {
        return await _loanService.SaveLoanRequestAsync(request.Request);
    }
}
