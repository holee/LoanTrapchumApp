using Application.Features.Locations.Dtos;
using Application.Interfaces.Lists;

namespace Application.Features.Locations.Queries.GetLocationsAsPaging;

public sealed class GetLocationsAsPagingQueryHandler(ILocationService locationService)
    : IRequestHandler<GetLocationsAsPagingQuery, Pager<LocationDto>>
{
    public async Task<Pager<LocationDto>> Handle(GetLocationsAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await locationService.GetPagedAsync(request.Request, cancellationToken);
    }
}
