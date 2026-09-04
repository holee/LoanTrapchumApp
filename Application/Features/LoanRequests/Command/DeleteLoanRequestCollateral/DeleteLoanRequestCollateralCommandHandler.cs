using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.DeleteLoanRequestCollateral;

public sealed class DeleteLoanRequestCollateralCommandHandler(ILoanRequestService _loanRequestService) : IRequestHandler<DeleteLoanRequestCollateralCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(DeleteLoanRequestCollateralCommand request, CancellationToken cancellationToken)
    {
        return await _loanRequestService.DeleteLoanRequestCollateralAsync(request.BranchId, request.LoanRequestId, request.CollateralLinkId);
    }
}