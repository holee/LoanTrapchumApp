using Applications.Commons;
using Applications.Features.Addresses.Dto;
using Applications.Responses.Paginations;

namespace Applications.Features.Addresses.Queries.GetAreas;

public record GetAreasQuery(long? Id, QueryParam Param) : IRequest<PageMetaData<AreaDto>>;
