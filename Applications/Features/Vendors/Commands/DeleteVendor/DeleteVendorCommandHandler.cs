using Applications.Interfaces.Vendors;
using ErrorOr;

namespace Applications.Features.Vendors.Commands.DeleteVendor;

public sealed class DeleteVendorCommandHandler(IVendorService vendorService)
    : IRequestHandler<DeleteVendorCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        return await vendorService.DeleteAsync(request.VendorId, cancellationToken);
    }
}
