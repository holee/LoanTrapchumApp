using ErrorOr;

namespace Applications.Features.Loans.Command.SaveLoanDisbursemenent;

public class SaveLoanDisbursementCommandHandler : IRequestHandler<SaveLoanDisbursementCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _loanService;
    public SaveLoanDisbursementCommandHandler(ILoanRequestService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<RequestResult>> Handle(SaveLoanDisbursementCommand request, CancellationToken cancellationToken)
    {
        return await _loanService.SaveLoanDisbursementAsync(request.Request);
    }
}
