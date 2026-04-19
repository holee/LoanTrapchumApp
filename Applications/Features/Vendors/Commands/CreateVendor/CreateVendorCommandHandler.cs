using Applications.Interfaces.Vendors;
using Applications.Responses.ViewModels.Vendors;
using ErrorOr;
using Mapster;

namespace Applications.Features.Vendors.Commands.CreateVendor;

public sealed class CreateVendorCommandHandler(IVendorService vendorService)
    : IRequestHandler<CreateVendorCommand, ErrorOr<VendorVm>>
{
    public async Task<ErrorOr<VendorVm>> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.Adapt<EntityNames>();
        return await vendorService.CreateAsync(entity, cancellationToken);
    }
}
