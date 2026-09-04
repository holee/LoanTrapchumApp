using Application.Features.Loans.Dtos.Responses;

namespace Application.Features.Loans.Queries.GetLoanRelatedGuarantors;

public sealed class GetLoanRelatedGuarantorsQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanRelatedGuarantorsQuery, IEnumerable<VLoanDisbursementGuarantors>>
{
    public async Task<IEnumerable<VLoanDisbursementGuarantors>> Handle(GetLoanRelatedGuarantorsQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetRelatedGaurantors(request.LoanId, request.BranchId, cancellationToken);
    }
}
