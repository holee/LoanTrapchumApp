using Application.Interfaces.Loans;
using Application.Responses.ViewModels.Loans.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestsDocuments;

public sealed class GetLoanRequestDocumentQueryHandler(ILoanRequestService loanRequestService)
    : IRequestHandler<GetLoanRequestDocumentQuery, ErrorOr<IEnumerable<VLoanRequestsDocuments>>>
{
    public async Task<ErrorOr<IEnumerable<VLoanRequestsDocuments>>> Handle(GetLoanRequestDocumentQuery request, CancellationToken cancellationToken)
    {
        return await loanRequestService.GetLoanRequestDocumentAsync(request.LoanRequestId, request.BranchId);
    }
}
