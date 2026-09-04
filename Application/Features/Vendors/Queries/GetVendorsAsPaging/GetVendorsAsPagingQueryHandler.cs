using Application.Features.Vendors.Dto.Responses;
using Application.Interfaces.Vendors;
using Application.Responses.Paginations;

namespace Application.Features.Vendors.Queries.GetVendorsAsPaging;

public sealed class GetVendorsAsPagingQueryHandler(IVendorService vendorService)
    : IRequestHandler<GetVendorsAsPagingQuery, Pager<VendorDto>>
{
    public async Task<Pager<VendorDto>> Handle(GetVendorsAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await vendorService.GetVendorsAsync(request.Param, cancellationToken);
    }
}
