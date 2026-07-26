using Applications.Interfaces.Addresses;
using Domains.Models;
using ErrorOr;
using Mapster;

namespace Applications.Features.Addresses.Command.UpdateArea;

public sealed class UpdateAreaCommandHandler(IAddressService addressService) : IRequestHandler<UpdateAreaCommand, ErrorOr<AreaDto?>>
{
    public async Task<ErrorOr<AreaDto?>> Handle(UpdateAreaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var area = request.Area.Adapt<Area>();

            return await addressService.AddOrUpdateAreasAsync(area, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the area: {ex.Message}", ex);
        }

    }
}
