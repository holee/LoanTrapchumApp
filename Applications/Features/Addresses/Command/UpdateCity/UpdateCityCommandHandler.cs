using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;
using Domains.Models.Addresses;
using ErrorOr;
using Mapster;

namespace Applications.Features.Addresses.Command.UpdateCity;

public sealed class UpdateCityCommandHandler(IAddressService addressService) : IRequestHandler<UpdateCityCommand, ErrorOr<CityDto>>
{
    public async Task<ErrorOr<CityDto>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {

        var city = request.City.Adapt<City>();

        return await addressService.UpdateCity(city, cancellationToken);

    }
}