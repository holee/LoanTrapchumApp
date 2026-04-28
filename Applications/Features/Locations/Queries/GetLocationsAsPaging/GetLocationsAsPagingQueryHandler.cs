using Applications.Features.Locations.Dtos;
using Applications.Interfaces.Lists;

namespace Applications.Features.Locations.Queries.GetLocationsAsPaging;

public sealed class GetLocationsAsPagingQueryHandler(ILocationService locationService)
    : IRequestHandler<GetLocationsAsPagingQuery, Pager<LocationDto>>
{
    public async Task<Pager<LocationDto>> Handle(GetLocationsAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await locationService.GetPagedAsync(request.Request, cancellationToken);
    }
}
