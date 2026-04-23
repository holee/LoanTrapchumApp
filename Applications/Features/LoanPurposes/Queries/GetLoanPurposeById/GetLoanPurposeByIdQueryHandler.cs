using Applications.Interfaces;

namespace Applications.Features.LoanPurposes.Queries.GetLoanPurposeById;

public sealed class GetLoanPurposeByIdQueryHandler(ILoanPurposeService loanPurposeService)
    : IRequestHandler<GetLoanPurposeByIdQuery, LoanPurposeDto?>
{
    public Task<LoanPurposeDto?> Handle(GetLoanPurposeByIdQuery request, CancellationToken cancellationToken)
        => loanPurposeService.GetByIdAsync(request.Id, cancellationToken);
}
