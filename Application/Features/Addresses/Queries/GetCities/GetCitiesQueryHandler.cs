using Application.Commons;
using Application.Features.Addresses.Dto;
using Application.Interfaces.Addresses;

namespace Application.Features.Addresses.Queries.GetCities;

public sealed class GetCitiesQueryHandler(IAddressService addressService) : IRequestHandler<GetCitiesQuery, PageMetaData<CityDto>>
{
    public async Task<PageMetaData<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetCities(request.CityId, request.Param, cancellationToken);
    }
}