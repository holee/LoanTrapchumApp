using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;

namespace Applications.Features.Vendors.Queries.GetVendorsAsPaging;

public record GetVendorsAsPagingQuery(QueryParam Param) : IRequest<Pager<VendorVm>>;
