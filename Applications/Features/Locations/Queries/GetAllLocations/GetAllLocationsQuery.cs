using Applications.Features.Locations.Dtos;

namespace Applications.Features.Locations.Queries.GetAllLocations;

public record GetAllLocationsQuery : IRequest<IEnumerable<LocationDto>>;
