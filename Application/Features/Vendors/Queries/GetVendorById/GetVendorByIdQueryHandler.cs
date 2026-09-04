using Application.Features.Vendors.Dto.Responses;
using Application.Interfaces.Vendors;
using ErrorOr;

namespace Application.Features.Vendors.Queries.GetVendorById;

public sealed class GetVendorByIdQueryHandler(IVendorService vendorService)
    : IRequestHandler<GetVendorByIdQuery, ErrorOr<VendorDto>>
{
    public async Task<ErrorOr<VendorDto>> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        return await vendorService.GetVendorByIdAsync(request.VendorId, cancellationToken);
    }
}
