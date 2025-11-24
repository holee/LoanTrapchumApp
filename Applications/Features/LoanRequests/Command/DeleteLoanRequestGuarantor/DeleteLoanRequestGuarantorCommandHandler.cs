using Applications.Interfaces.Loans;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanRequests.Command.DeleteLoanRequestGuarantor;

public sealed class DeleteLoanRequestGuarantorCommandHandler(ILoanRequestService _loanRequestService)
    : IRequestHandler<DeleteLoanRequestGuarantorCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteLoanRequestGuarantorCommand request, CancellationToken cancellationToken)
    {
        return await _loanRequestService.DeleteLoanRequestGuarantorAsync(
            request.LoanRequestId,
            request.BranchId,
            request.GuarantorLinkId);
    }
}
