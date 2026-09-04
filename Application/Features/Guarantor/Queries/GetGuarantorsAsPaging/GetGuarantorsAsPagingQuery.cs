using Application.Features.Guarantor.Dtos;
using Application.Responses.Paginations;

namespace Application.Features.Guarantor.Queries.GetGuarantorsAsPaging;

public record GetGuarantorsAsPagingQuery(string? Id, QueryParam Param) : IRequest<Pager<VGuarantor>>;
