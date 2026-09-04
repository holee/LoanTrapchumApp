using Application.Features.Locations.Dtos;
using Application.Responses.Paginations;

namespace Application.Features.Locations.Queries.GetLocationsAsPaging;

public record GetLocationsAsPagingQuery(QueryParam Request) : IRequest<Pager<LocationDto>>;
