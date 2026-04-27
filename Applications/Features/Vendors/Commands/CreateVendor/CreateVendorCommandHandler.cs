using Applications.Commons.Mappers;
using Applications.Features.Vendors.Dto.Responses;
using Applications.Interfaces.Vendors;
using ErrorOr;
using Mapster;

namespace Applications.Features.Vendors.Commands.CreateVendor;

public sealed class CreateVendorCommandHandler(IVendorService vendorService) : IRequestHandler<CreateVendorCommand, ErrorOr<VendorDto>>
{
    public async Task<ErrorOr<VendorDto>> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToVendorCreate();

        return await vendorService.CreateAsync(entity, cancellationToken);
    }
}
