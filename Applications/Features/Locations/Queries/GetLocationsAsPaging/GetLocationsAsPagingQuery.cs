using Applications.Features.Locations.Dtos;
using Applications.Responses.Paginations;

namespace Applications.Features.Locations.Queries.GetLocationsAsPaging;

public record GetLocationsAsPagingQuery(QueryParam Request) : IRequest<Pager<LocationDto>>;
