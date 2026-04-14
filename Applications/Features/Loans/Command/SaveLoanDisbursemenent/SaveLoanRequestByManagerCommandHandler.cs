using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.Loans.Command.SaveLoanDisbursemenent;

public class SaveLoanRequestByManagerCommandHandler : IRequestHandler<SaveLoanRequestByManagerCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _loanService;
    public SaveLoanRequestByManagerCommandHandler(ILoanRequestService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<RequestResult>> Handle(SaveLoanRequestByManagerCommand request, CancellationToken cancellationToken)
    {
        return await _loanService.SaveLoanRequestByManagerAsync(request.Request);
    }
}
