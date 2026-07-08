using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.Loans.Queries.GetLoanDetailsById;

public sealed class GetLoanDetialsByIdQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanDetailsByIdQuery, ErrorOr<VLoanDisbursments>>
{
    public async Task<ErrorOr<VLoanDisbursments>> Handle(GetLoanDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetLoanByIdAsync(request.LoanId, request.BranchId, request.Status, request.Include,cancellationToken);
    }
}
