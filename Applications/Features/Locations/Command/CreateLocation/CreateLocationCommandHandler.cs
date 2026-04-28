using Applications.Features.Locations.Dtos;
using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.Locations.Command.CreateLocation;

public sealed class CreateLocationCommandHandler(ILocationService locationService)
    : IRequestHandler<CreateLocationCommand, ErrorOr<LocationDto>>
{
    public async Task<ErrorOr<LocationDto>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Request.ToLocation();
        return await locationService.CreateAsync(entity, cancellationToken);
    }
}
