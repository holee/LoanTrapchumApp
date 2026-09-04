using Application.Commons;
using Application.Features.Addresses.Dto;

namespace Application.Features.Addresses.Queries.GetVillages;

public record GetVillagesQuery(int? communeId, int? villagerId, QueryParam Param) : IRequest<PageMetaData<VillageDto>>;
