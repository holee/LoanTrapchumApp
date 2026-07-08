using Domains.Models.Loans;
using ErrorOr;

namespace Applications.Features.Loans.Queries.GetLoanById;

public sealed class GetLoanByIdQueryHandler : IRequestHandler<GetLoanByIdQuery, ErrorOr<LoanDisbursment>>
{
    private readonly ILoanService _loanService;
    public GetLoanByIdQueryHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<LoanDisbursment>> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _loanService.GetLoanByIdAsync(request.LoanId, request.BranchId, cancellationToken);

            if (loan is null)
            {
                return Error.NotFound("Loan not found.");
            }

            return loan;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred while retrieving the loan: {ex.Message}");
        }


    }
}