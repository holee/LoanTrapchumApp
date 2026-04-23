using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.LoanPurposes.Commands.UpdateLoanPurpose;

public sealed class UpdateLoanPurposeCommandHandler(ILoanPurposeService loanPurposeService)
    : IRequestHandler<UpdateLoanPurposeCommand, ErrorOr<LoanPurposeDto>>
{
    public Task<ErrorOr<LoanPurposeDto>> Handle(UpdateLoanPurposeCommand request, CancellationToken cancellationToken)
        => loanPurposeService.UpdateAsync(request.Request, cancellationToken);
}
