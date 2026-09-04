using Application.Features.Locations.Dtos;

namespace Application.Features.Locations.Queries.GetAllLocations;

public record GetAllLocationsQuery : IRequest<IEnumerable<LocationDto>>;
