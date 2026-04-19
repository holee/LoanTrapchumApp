using Applications.Interfaces.Vendors;
using Applications.Responses.ViewModels.Vendors;
using ErrorOr;

namespace Applications.Features.Vendors.Queries.GetVendorById;

public sealed class GetVendorByIdQueryHandler(IVendorService vendorService)
    : IRequestHandler<GetVendorByIdQuery, ErrorOr<VendorVm>>
{
    public async Task<ErrorOr<VendorVm>> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        return await vendorService.GetVendorByIdAsync(request.VendorId, cancellationToken);
    }
}
