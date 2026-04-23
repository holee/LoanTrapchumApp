using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.LoanPurposes.Commands.CreateLoanPurpose;

public sealed class CreateLoanPurposeCommandHandler(ILoanPurposeService loanPurposeService)
    : IRequestHandler<CreateLoanPurposeCommand, ErrorOr<LoanPurposeDto>>
{
    public Task<ErrorOr<LoanPurposeDto>> Handle(CreateLoanPurposeCommand request, CancellationToken cancellationToken)
        => loanPurposeService.CreateAsync(request.Request, cancellationToken);
}
