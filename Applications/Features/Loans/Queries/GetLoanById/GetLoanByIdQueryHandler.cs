using ErrorOr;

namespace Applications.Features.Loans.Queries.GetLoanById;

public sealed class GetLoanByIdQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanByIdQuery, ErrorOr<VLoanDisbursments>>
{
    public async Task<ErrorOr<VLoanDisbursments>> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetLoanByIdAsync(request.LoanId, request.BranchId, request.Status, request.Include,cancellationToken);
    }
}
