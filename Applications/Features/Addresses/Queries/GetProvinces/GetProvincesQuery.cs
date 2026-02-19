using Applications.Commons;
using Applications.Features.Addresses.Dto;

namespace Applications.Features.Addresses.Queries.GetProvinces;

public record GetProvincesQuery(int? provinceId, QueryParam Param) : IRequest<PageMetaData<ProvinceDto>>;
