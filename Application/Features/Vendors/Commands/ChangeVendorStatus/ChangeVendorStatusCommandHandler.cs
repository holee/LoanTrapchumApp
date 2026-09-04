using Application.Interfaces.Vendors;
using ErrorOr;

namespace Application.Features.Vendors.Commands.ChangeVendorStatus;

public sealed class ChangeVendorStatusCommandHandler(IVendorService vendorService)
    : IRequestHandler<ChangeVendorStatusCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(ChangeVendorStatusCommand request, CancellationToken cancellationToken)
    {
        return await vendorService.ChangeVendorStatusAsync(request.VendorId, request.IsActive, cancellationToken);
    }
}
