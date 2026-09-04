using Application.Responses.Paginations;

namespace Application.Features.LoanPurposes.Queries.GetLoanPurposes;

public record GetLoanPurposesQuery(QueryParam? Param) : IRequest<Pager<LoanPurposeDto>>;
