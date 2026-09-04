using Application.Features.Locations.Dtos;
using ErrorOr;

namespace Application.Features.Locations.Command.CreateLocation;

public record CreateLocationCommand(LocationCreateDto Request) : IRequest<ErrorOr<LocationDto>>;
