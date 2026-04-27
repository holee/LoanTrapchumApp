using Applications.Features.Vendors.Dto.Responses;
using Applications.Interfaces.Vendors;
using Applications.Responses.Paginations;

namespace Applications.Features.Vendors.Queries.GetVendorsAsPaging;

public sealed class GetVendorsAsPagingQueryHandler(IVendorService vendorService)
    : IRequestHandler<GetVendorsAsPagingQuery, Pager<VendorDto>>
{
    public async Task<Pager<VendorDto>> Handle(GetVendorsAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await vendorService.GetVendorsAsync(request.Param, cancellationToken);
    }
}
