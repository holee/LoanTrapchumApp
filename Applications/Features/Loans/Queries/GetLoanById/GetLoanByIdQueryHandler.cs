using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanById;

public sealed class GetLoanByIdQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanByIdQuery, Result<VLoanDisbursments>>
{ 
    public async Task<Result<VLoanDisbursments>> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetLoanByIdAsync(request.LoanId, request.BranchId, request.Status, request.Include);
    }
}
