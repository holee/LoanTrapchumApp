using Application.Interfaces.Addresses;

namespace Application.Features.Addresses.Queries.GetAreas;

public sealed class GetAreasQueryHandler(IAddressService addressService) : IRequestHandler<GetAreasQuery, PageMetaData<AreaDto>>
{
    public async Task<PageMetaData<AreaDto>> Handle(GetAreasQuery request, CancellationToken cancellationToken)
    {
        return await addressService.GetAreasAsync(request.Id, request.Param, cancellationToken);
    }
}
