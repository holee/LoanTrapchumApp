using Applications.Interfaces.Vendors;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Vendors;

namespace Applications.Features.Vendors.Queries.GetVendorsAsPaging;

public sealed class GetVendorsAsPagingQueryHandler(IVendorService vendorService)
    : IRequestHandler<GetVendorsAsPagingQuery, Pager<VendorVm>>
{
    public async Task<Pager<VendorVm>> Handle(GetVendorsAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await vendorService.GetVendorsAsync(request.Param, cancellationToken);
    }
}
