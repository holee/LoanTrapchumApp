using Applications.Commons;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;

namespace Applications.Features.Addresses.Queries.GetProvinces;

public sealed class GetProvincesQueryHandler(IAddressService addressService) : IRequestHandler<GetProvincesQuery, PageMetaData<ProvinceDto>>
{
    public async Task<PageMetaData<ProvinceDto>> Handle(GetProvincesQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetProvinces(request.provinceId, request.Param, cancellationToken);
    }
}