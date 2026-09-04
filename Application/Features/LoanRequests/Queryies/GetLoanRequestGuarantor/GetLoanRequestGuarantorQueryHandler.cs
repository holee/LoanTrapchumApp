using Application.Interfaces.Loans;
using Application.Responses.ViewModels.Loans.Requests;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Queryies.GetLoanRequestGuarantor;

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