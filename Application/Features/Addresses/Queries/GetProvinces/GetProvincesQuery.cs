using Application.Commons;
using Application.Features.Addresses.Dto;

namespace Application.Features.Addresses.Queries.GetProvinces;

public record GetProvincesQuery(int? provinceId, QueryParam Param) : IRequest<PageMetaData<ProvinceDto>>;
