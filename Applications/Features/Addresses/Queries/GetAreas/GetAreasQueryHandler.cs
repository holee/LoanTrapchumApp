using Applications.Commons;
using Applications.Features.Addresses.Dto;
using Applications.Interfaces.Addresses;

namespace Applications.Features.Addresses.Queries.GetAreas;

public sealed class GetAreasQueryHandler(IAddressService addressService) : IRequestHandler<GetAreasQuery, PageMetaData<AreaDto>>
{
    public async Task<PageMetaData<AreaDto>> Handle(GetAreasQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetAreasAsync(request.Id, request.Param, cancellationToken);
    }
}
