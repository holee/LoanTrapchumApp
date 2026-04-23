using Applications.Interfaces;
using Applications.Responses.Paginations;

namespace Applications.Features.LoanPurposes.Queries.GetLoanPurposes;

public sealed class GetLoanPurposesQueryHandler(ILoanPurposeService loanPurposeService)
    : IRequestHandler<GetLoanPurposesQuery, Pager<LoanPurposeDto>>
{
    public Task<Pager<LoanPurposeDto>> Handle(GetLoanPurposesQuery request, CancellationToken cancellationToken)
        => loanPurposeService.GetAllAsync(request.Param, cancellationToken);
}
