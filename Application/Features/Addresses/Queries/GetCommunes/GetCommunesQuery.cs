using Application.Commons;
using Application.Features.Addresses.Dto;

namespace Application.Features.Addresses.Queries.GetCommunes;

public record GetCommunesQuery(int? DistrictId, int? CommuneId, QueryParam Param) : IRequest<PageMetaData<CommuneDto>>;
