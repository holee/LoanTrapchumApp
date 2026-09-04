using Application.Features.Addresses.Dto;
using Application.Interfaces.Addresses;
using Domains.Models.Addresses;
using ErrorOr;
using Mapster;

namespace Application.Features.Addresses.Command.UpdateCity;

public sealed class UpdateCityCommandHandler(IAddressService addressService) : IRequestHandler<UpdateCityCommand, ErrorOr<CityDto>>
{
    public async Task<ErrorOr<CityDto>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {

        var city = request.City.Adapt<City>();

        return await addressService.UpdateCity(city, cancellationToken);

    }
}