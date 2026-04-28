using Applications.Features.Locations.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Locations.Command.UpdateLocation;

public sealed class UpdateLocationCommandHandler(ILocationService locationService)
    : IRequestHandler<UpdateLocationCommand, ErrorOr<LocationDto>>
{
    public async Task<ErrorOr<LocationDto>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Request.ToLocation();
        return await locationService.UpdateAsync(entity, cancellationToken);
    }
}
