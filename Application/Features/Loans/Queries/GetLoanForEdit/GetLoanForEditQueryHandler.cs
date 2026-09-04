using ErrorOr;

namespace Application.Features.Loans.Queries.GetLoanForEdit;

public class GetLoanForEditQueryHandler : IRequestHandler<GetLoanForEditQuery, ErrorOr<LoanDtoCreate>>
{
    private readonly ILoanService _loanRepository;

    public GetLoanForEditQueryHandler(ILoanService loanRepository)
    {
        _loanRepository = loanRepository;
    }
    public async Task<ErrorOr<LoanDtoCreate>> Handle(GetLoanForEditQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var loan = await _loanRepository.GetLoanForEditAsync(request.LoanId, request.BranchId, cancellationToken);

            if (loan == null)
            {
                return Error.NotFound($"Loan with ID {request.LoanId} not found.");
            }

            return loan;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred while retrieving the loan: {ex.Message}");
        }

    }
}