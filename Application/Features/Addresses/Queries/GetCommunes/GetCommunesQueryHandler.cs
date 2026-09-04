using Application.Commons;
using Application.Features.Addresses.Dto;
using Application.Interfaces.Addresses;

namespace Application.Features.Addresses.Queries.GetCommunes;

public sealed class GetCommunesQueryHandler(IAddressService addressService) : IRequestHandler<GetCommunesQuery, PageMetaData<CommuneDto>>
{
    public async Task<PageMetaData<CommuneDto>> Handle(GetCommunesQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetCommunesByDistrictId(request.DistrictId, request.CommuneId, request.Param, cancellationToken);
    }
}