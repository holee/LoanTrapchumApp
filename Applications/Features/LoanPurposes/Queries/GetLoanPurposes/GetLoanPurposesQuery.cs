using Applications.Responses.Paginations;

namespace Applications.Features.LoanPurposes.Queries.GetLoanPurposes;

public record GetLoanPurposesQuery(QueryParam? Param) : IRequest<Pager<LoanPurposeDto>>;
