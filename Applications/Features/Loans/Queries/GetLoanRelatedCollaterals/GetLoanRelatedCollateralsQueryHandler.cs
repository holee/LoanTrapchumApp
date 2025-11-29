using Applications.Features.Collaterals.Dtos;
using Applications.Interfaces.Loans;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanRelatedCollaterals;

public sealed class GetLoanRelatedCollateralsQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanRelatedCollateralsQuery, IEnumerable<VCollateralLines>>
{
    public async Task<IEnumerable<VCollateralLines>> Handle(GetLoanRelatedCollateralsQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetRelatedCollaterals(request.LoanId, request.BranchId,cancellationToken);
    }
}
