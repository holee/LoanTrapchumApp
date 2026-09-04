using Application.Interfaces.Lists;
using Domains.Models.Lists.Vendors;
using ErrorOr;
using Mapster;

namespace Application.Features.OtherNames.Commands.ConvertToVendor;

public sealed class ConvertToVendorCommandHandler(IOtherNameService otherNameService)
    : IRequestHandler<ConvertToVendorCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ConvertToVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = request.Dto.Adapt<Vendor>();
        return await otherNameService.ConvertToVendorAsync(request.Id, vendor, cancellationToken);
    }
}
