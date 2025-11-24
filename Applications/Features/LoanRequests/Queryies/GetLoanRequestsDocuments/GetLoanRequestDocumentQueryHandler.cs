using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;

public sealed class GetLoanRequestDocumentQueryHandler(ILoanRequestService loanRequestService)
    : IRequestHandler<GetLoanRequestDocumentQuery, ErrorOr<IEnumerable<VLoanRequestsDocuments>>>
{
    public async Task<ErrorOr<IEnumerable<VLoanRequestsDocuments>>> Handle(GetLoanRequestDocumentQuery request, CancellationToken cancellationToken)
    {

        return await loanRequestService.GetLoanRequestDocumentAsync(request.LoanRequestId, request.BranchId);
    }
}
