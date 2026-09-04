using Application.Commons;
using Application.Features.Addresses.Dto;
using Application.Interfaces.Addresses;

namespace Application.Features.Addresses.Queries.GetProvinces;

public sealed class GetProvincesQueryHandler(IAddressService addressService) : IRequestHandler<GetProvincesQuery, PageMetaData<ProvinceDto>>
{
    public async Task<PageMetaData<ProvinceDto>> Handle(GetProvincesQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetProvinces(request.provinceId, request.Param, cancellationToken);
    }
}