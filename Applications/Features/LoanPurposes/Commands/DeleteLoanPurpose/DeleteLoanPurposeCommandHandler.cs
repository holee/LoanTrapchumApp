using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.LoanPurposes.Commands.DeleteLoanPurpose;

public sealed class DeleteLoanPurposeCommandHandler(ILoanPurposeService loanPurposeService)
    : IRequestHandler<DeleteLoanPurposeCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(DeleteLoanPurposeCommand request, CancellationToken cancellationToken)
        => loanPurposeService.DeleteAsync(request.Id, cancellationToken);
}
