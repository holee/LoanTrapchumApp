using Applications.Features.Vendors.Dto.Responses;
using Applications.Responses.Paginations;

namespace Applications.Features.Vendors.Queries.GetVendorsAsPaging;

public record GetVendorsAsPagingQuery(QueryParam Param) : IRequest<Pager<VendorDto>>;
