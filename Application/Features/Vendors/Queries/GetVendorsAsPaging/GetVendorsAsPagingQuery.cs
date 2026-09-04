using Application.Features.Vendors.Dto.Responses;
using Application.Responses.Paginations;

namespace Application.Features.Vendors.Queries.GetVendorsAsPaging;

public record GetVendorsAsPagingQuery(QueryParam Param) : IRequest<Pager<VendorDto>>;
