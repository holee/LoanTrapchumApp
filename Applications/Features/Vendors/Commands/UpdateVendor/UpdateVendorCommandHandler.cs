using Applications.Interfaces.Vendors;
using Applications.Responses.ViewModels.Vendors;
using Domains.Models.Lists;
using ErrorOr;
using Mapster;

namespace Applications.Features.Vendors.Commands.UpdateVendor;

public sealed class UpdateVendorCommandHandler(IVendorService vendorService)
    : IRequestHandler<UpdateVendorCommand, ErrorOr<VendorVm>>
{
    public async Task<ErrorOr<VendorVm>> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.Adapt<EntityNames>();
        return await vendorService.UpdateAsync(entity, cancellationToken);
    }
}
