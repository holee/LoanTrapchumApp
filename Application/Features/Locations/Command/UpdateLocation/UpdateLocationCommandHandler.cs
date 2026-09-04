using Application.Features.Locations.Dtos;
using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.Locations.Command.UpdateLocation;

public sealed class UpdateLocationCommandHandler(ILocationService locationService)
    : IRequestHandler<UpdateLocationCommand, ErrorOr<LocationDto>>
{
    public async Task<ErrorOr<LocationDto>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Request.ToLocation();
        return await locationService.UpdateAsync(entity, cancellationToken);
    }
}
