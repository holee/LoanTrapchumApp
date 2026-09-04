using Application.Commons;
using Application.Features.Addresses.Dto;
using Application.Interfaces.Addresses;

namespace Application.Features.Addresses.Queries.GetDistricts;

public sealed class GetDistrictsQueryHandler(IAddressService addressService) : IRequestHandler<GetDistrictsQuery, PageMetaData<DistrictDto>>
{
    public async Task<PageMetaData<DistrictDto>> Handle(GetDistrictsQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetDistrictsByProvinceId(request.ProvinceId, request.DistrictId, request.Param, cancellationToken);
    }
}