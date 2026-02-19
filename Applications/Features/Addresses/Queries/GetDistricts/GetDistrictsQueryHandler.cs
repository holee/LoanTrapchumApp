using Applications.Commons;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;

namespace Applications.Features.Addresses.Queries.GetDistricts;

public sealed class GetDistrictsQueryHandler(IAddressService addressService) : IRequestHandler<GetDistrictsQuery, PageMetaData<DistrictDto>>
{
    public async Task<PageMetaData<DistrictDto>> Handle(GetDistrictsQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetDistrictsByProvinceId(request.ProvinceId, request.DistrictId, request.Param, cancellationToken);
    }
}