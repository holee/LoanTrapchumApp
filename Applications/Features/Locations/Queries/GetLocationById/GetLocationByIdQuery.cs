using Applications.Features.Locations.Dtos;
using ErrorOr;

namespace Applications.Features.Locations.Queries.GetLocationById;

public record GetLocationByIdQuery(long Id) : IRequest<ErrorOr<LocationDto>>;
