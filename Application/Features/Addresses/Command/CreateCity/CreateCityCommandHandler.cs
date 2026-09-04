using Application.Features.Addresses.Dto;
using Application.Interfaces.Addresses;
using Domains.Models.Addresses;
using ErrorOr;
using Mapster;

namespace Application.Features.Addresses.Command.CreateCity;

public sealed class CreateCityCommandHandler(IAddressService addressService) : IRequestHandler<CreateCityCommand, ErrorOr<CityDto>>
{
    public async Task<ErrorOr<CityDto>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {

        var city = request.City.Adapt<City>();

        return await addressService.AddCity(city, cancellationToken);

    }
}