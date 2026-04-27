using Applications.Features.Vendors.Dto.Responses;
using Applications.Interfaces.Vendors;
using ErrorOr;

namespace Applications.Features.Vendors.Queries.GetVendorById;

public sealed class GetVendorByIdQueryHandler(IVendorService vendorService)
    : IRequestHandler<GetVendorByIdQuery, ErrorOr<VendorDto>>
{
    public async Task<ErrorOr<VendorDto>> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        return await vendorService.GetVendorByIdAsync(request.VendorId, cancellationToken);
    }
}
