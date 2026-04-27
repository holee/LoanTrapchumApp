using Applications.Features.Vendors.Dto.Responses;
using Applications.Interfaces.Vendors;
using ErrorOr;
using Mapster;

namespace Applications.Features.Vendors.Commands.UpdateVendor;

public sealed class UpdateVendorCommandHandler(IVendorService vendorService)
    : IRequestHandler<UpdateVendorCommand, ErrorOr<VendorDto>>
{
    public async Task<ErrorOr<VendorDto>> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.Adapt<EntityNames>();

        return await vendorService.UpdateAsync(entity, cancellationToken);
    }
}
