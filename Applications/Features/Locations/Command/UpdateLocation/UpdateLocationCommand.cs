using Applications.Features.Locations.Dtos;
using ErrorOr;

namespace Applications.Features.Locations.Command.UpdateLocation;

public record UpdateLocationCommand(LocationCreateDto Request) : IRequest<ErrorOr<LocationDto>>;
