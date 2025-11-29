using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanRelatedGuarantors;

public sealed class GetLoanRelatedGuarantorsQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanRelatedGuarantorsQuery, IEnumerable<VLoanGuarantors>>
{
    public async Task<IEnumerable<VLoanGuarantors>> Handle(GetLoanRelatedGuarantorsQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetRelatedGaurantors(request.LoanId, request.BranchId, cancellationToken);
    }
}
