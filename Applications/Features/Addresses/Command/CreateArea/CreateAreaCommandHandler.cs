using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Domains.Models;
using ErrorOr;
using Mapster;

namespace Applications.Features.Addresses.Command.CreateArea;

public sealed class CreateAreaCommandHandler(IAddressService addressService) : IRequestHandler<CreateAreaCommand, ErrorOr<AreaDto?>>
{
    public async Task<ErrorOr<AreaDto?>> Handle(CreateAreaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var area = request.Area.Adapt<Area>();
            area.AreaCode = area.VillageId.ToString();
            var result = await addressService.AddOrUpdateAreasAsync(area, cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while creating the area: {ex.Message}", ex);
        }

    }
}
