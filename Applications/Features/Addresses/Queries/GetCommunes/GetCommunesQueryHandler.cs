using Applications.Commons;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;

namespace Applications.Features.Addresses.Queries.GetCommunes;

public sealed class GetCommunesQueryHandler(IAddressService addressService) : IRequestHandler<GetCommunesQuery, PageMetaData<CommuneDto>>
{
    public async Task<PageMetaData<CommuneDto>> Handle(GetCommunesQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetCommunesByDistrictId(request.DistrictId, request.CommuneId, request.Param, cancellationToken);
    }
}