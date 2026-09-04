using Application.Commons;
using Application.Features.Addresses.Dto;

namespace Application.Features.Addresses.Queries.GetDistricts;

public record GetDistrictsQuery(int? ProvinceId, int? DistrictId, QueryParam Param) : IRequest<PageMetaData<DistrictDto>>;
