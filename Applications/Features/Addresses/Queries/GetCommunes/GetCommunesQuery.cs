using Applications.Commons;
using Applications.Features.Addresses.Dto;

namespace Applications.Features.Addresses.Queries.GetCommunes;

public record GetCommunesQuery(int? DistrictId, int? CommuneId, QueryParam Param) : IRequest<PageMetaData<CommuneDto>>;
