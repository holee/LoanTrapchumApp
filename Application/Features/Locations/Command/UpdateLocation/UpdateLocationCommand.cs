using Application.Features.Locations.Dtos;
using ErrorOr;

namespace Application.Features.Locations.Command.UpdateLocation;

public record UpdateLocationCommand(LocationCreateDto Request) : IRequest<ErrorOr<LocationDto>>;
