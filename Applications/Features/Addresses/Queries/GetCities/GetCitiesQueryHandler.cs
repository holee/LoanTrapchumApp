using Applications.Commons;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;

namespace Applications.Features.Addresses.Queries.GetCities;

public sealed class GetCitiesQueryHandler(IAddressService addressService) : IRequestHandler<GetCitiesQuery, PageMetaData<CityDto>>
{
    public async Task<PageMetaData<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetCities(request.CityId, request.Param, cancellationToken);
    }
}