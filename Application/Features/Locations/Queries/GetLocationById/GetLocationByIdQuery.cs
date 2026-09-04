using Application.Features.Locations.Dtos;
using ErrorOr;

namespace Application.Features.Locations.Queries.GetLocationById;

public record GetLocationByIdQuery(long Id) : IRequest<ErrorOr<LocationDto>>;
