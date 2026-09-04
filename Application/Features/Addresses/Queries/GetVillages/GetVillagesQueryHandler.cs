using Application.Commons;
using Application.Features.Addresses.Dto;
using Application.Interfaces.Addresses;

namespace Application.Features.Addresses.Queries.GetVillages;

public sealed class GetVillagesQueryHandler(IAddressService addressService) : IRequestHandler<GetVillagesQuery, PageMetaData<VillageDto>>
{
    public async Task<PageMetaData<VillageDto>> Handle(GetVillagesQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetVillagesByCommuneId(request.communeId, request.villagerId, request.Param, cancellationToken);
    }
}