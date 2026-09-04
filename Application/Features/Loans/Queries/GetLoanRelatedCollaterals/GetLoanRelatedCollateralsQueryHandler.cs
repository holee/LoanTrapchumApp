using Application.Features.Loans.Dtos.Responses;

namespace Application.Features.Loans.Queries.GetLoanRelatedCollaterals;

public sealed class GetLoanRelatedCollateralsQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanRelatedCollateralsQuery, IEnumerable<VLoanDisbursementCollateral>>
{
    public async Task<IEnumerable<VLoanDisbursementCollateral>> Handle(GetLoanRelatedCollateralsQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetRelatedCollaterals(request.LoanId, request.BranchId,cancellationToken);
    }
}
