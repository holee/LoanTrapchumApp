using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanRelatedDocuments;

public sealed class GetLoanRelatedDocumentsQueryHandler(ILoanService loanService) : IRequestHandler<GetLoanRelatedDocumentsQuery, IEnumerable<VLoanRequestsDocuments>>
{
    public async Task<IEnumerable<VLoanRequestsDocuments>> Handle(GetLoanRelatedDocumentsQuery request, CancellationToken cancellationToken)
    {
        return await loanService.GetRelatedDocuments(request.LoanId, request.BranchId, cancellationToken);
    }
}
