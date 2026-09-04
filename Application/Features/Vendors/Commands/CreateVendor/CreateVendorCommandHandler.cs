using Application.Commons.Mappers;
using Application.Features.Vendors.Dto.Responses;
using Application.Interfaces.Vendors;
using ErrorOr;
using Mapster;

namespace Application.Features.Vendors.Commands.CreateVendor;

public sealed class CreateVendorCommandHandler(IVendorService vendorService) : IRequestHandler<CreateVendorCommand, ErrorOr<VendorDto>>
{
    public async Task<ErrorOr<VendorDto>> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToVendorCreate();

        return await vendorService.CreateAsync(entity, cancellationToken);
    }
}
