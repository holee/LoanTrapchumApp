using Application.Features.Loans.Dtos.Responses;

namespace Application.Features.Loans.Queries.GetLoanRelatedDocuments;

public sealed class GetLoanRelatedDocumentsQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanRelatedDocumentsQuery, IEnumerable<VLoanDisbursementDocuments>>
{
    public async Task<IEnumerable<VLoanDisbursementDocuments>> Handle(GetLoanRelatedDocumentsQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetRelatedDocuments(request.LoanId, request.BranchId, cancellationToken);
    }
}
