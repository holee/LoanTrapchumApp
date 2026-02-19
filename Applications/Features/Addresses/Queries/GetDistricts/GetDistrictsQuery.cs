using Applications.Commons;
using Applications.Features.Addresses.Dto;

namespace Applications.Features.Addresses.Queries.GetDistricts;

public record GetDistrictsQuery(int? ProvinceId, int? DistrictId, QueryParam Param) : IRequest<PageMetaData<DistrictDto>>;
