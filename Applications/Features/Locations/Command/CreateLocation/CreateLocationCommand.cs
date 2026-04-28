using Applications.Features.Locations.Dtos;
using ErrorOr;

namespace Applications.Features.Locations.Command.CreateLocation;

public record CreateLocationCommand(LocationCreateDto Request) : IRequest<ErrorOr<LocationDto>>;
