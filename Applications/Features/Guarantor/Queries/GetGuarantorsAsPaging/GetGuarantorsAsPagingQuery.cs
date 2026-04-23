using Applications.Features.Guarantor.Dtos;
using Applications.Responses.Paginations;

namespace Applications.Features.Guarantor.Queries.GetGuarantorsAsPaging;

public record GetGuarantorsAsPagingQuery(string? Id, QueryParam Param) : IRequest<Pager<VGuarantor>>;
