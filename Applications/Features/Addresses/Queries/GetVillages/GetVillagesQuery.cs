using Applications.Commons;
using Applications.Features.Addresses.Dto;

namespace Applications.Features.Addresses.Queries.GetVillages;

public record GetVillagesQuery(int? communeId, int? villagerId, QueryParam Param) : IRequest<PageMetaData<VillageDto>>;
