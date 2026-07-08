using ErrorOr;

namespace Applications.Features.Loans.Command.DeleteLoan;

public sealed class DeleteLoanCommandHandler : IRequestHandler<DeleteLoanCommand, ErrorOr<DeleteResult>>
{

    private readonly ILoanService _loanService;

    public DeleteLoanCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;

    }
    public async Task<ErrorOr<DeleteResult>> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _loanService.DeleteLoanDisbursementAsync(request.LoanId, request.BranchId, request.CustomerId, cancellationToken);
        }
        catch (Exception ex)
        {
            return Error.Unexpected($"An unexpected error occurred while deleting the loan disbursement: {ex.Message}");
        }

    }
}