using Applications.Commons;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;

namespace Applications.Features.Addresses.Queries.GetVillages;

public sealed class GetVillagesQueryHandler(IAddressService addressService) : IRequestHandler<GetVillagesQuery, PageMetaData<VillageDto>>
{
    public async Task<PageMetaData<VillageDto>> Handle(GetVillagesQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetVillagesByCommuneId(request.communeId, request.villagerId, request.Param, cancellationToken);
    }
}