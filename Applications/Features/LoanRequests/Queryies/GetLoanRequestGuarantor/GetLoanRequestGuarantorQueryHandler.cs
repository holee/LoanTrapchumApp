using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Requests;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Queryies.GetLoanRequestGuarantor;

public sealed class GetLoanRequestGuarantorQueryHandler(ILoanRequestService loanRequestRepository) : IRequestHandler<GetLoanRequestGuarantorQuery, ErrorOr<IEnumerable<VLoanRequestGuarantors>>>
{
    public async Task<ErrorOr<IEnumerable<VLoanRequestGuarantors>>> Handle(GetLoanRequestGuarantorQuery request, CancellationToken cancellationToken)
    {
        return await loanRequestRepository.GetLoanRequestGuarantorAsync(
            request.BranchId,
            request.LoanRequestId,
            request.CustomerId);

    }
}