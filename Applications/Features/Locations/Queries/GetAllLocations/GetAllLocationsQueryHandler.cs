using Applications.Features.Locations.Dtos;
using Applications.Interfaces.Lists;

namespace Applications.Features.Locations.Queries.GetAllLocations;

public sealed class GetAllLocationsQueryHandler(ILocationService locationService)
    : IRequestHandler<GetAllLocationsQuery, IEnumerable<LocationDto>>
{
    public async Task<IEnumerable<LocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
    {
        return await locationService.GetAllAsync(cancellationToken);
    }
}
