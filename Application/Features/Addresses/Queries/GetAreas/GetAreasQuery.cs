using Application.Commons;
using Application.Features.Addresses.Dto;
using Application.Responses.Paginations;

namespace Application.Features.Addresses.Queries.GetAreas;

public record GetAreasQuery(long? Id, QueryParam Param) : IRequest<PageMetaData<AreaDto>>;
