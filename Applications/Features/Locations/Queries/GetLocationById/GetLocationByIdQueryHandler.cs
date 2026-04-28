using Applications.Features.Locations.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Locations.Queries.GetLocationById;

public sealed class GetLocationByIdQueryHandler(ILocationService locationService)
    : IRequestHandler<GetLocationByIdQuery, ErrorOr<LocationDto>>
{
    public async Task<ErrorOr<LocationDto>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
    {
        return await locationService.GetByIdAsync(request.Id, cancellationToken);
    }
}
