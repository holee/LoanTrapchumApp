using Applications.Interfaces.Loans;
using ErrorOr;

namespace Applications.Features.Loans.Command.CloseLoanDisbursment;

public class CloseLoanDisbursementCommandHandler : IRequestHandler<CloseLoanDisbursementCommand, ErrorOr<string>>
{
    private readonly ILoanService _loanService;
    public CloseLoanDisbursementCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<string>> Handle(CloseLoanDisbursementCommand request, CancellationToken cancellationToken)
    {
        return await _loanService.CloseLoanDisbursementAsync(request.BranchId, request.LoanId, request.CustomerId, cancellationToken);
    }
}