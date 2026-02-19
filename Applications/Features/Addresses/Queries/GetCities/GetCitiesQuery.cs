using Applications.Commons;
using Applications.Features.Addresses.Dto;

namespace Applications.Features.Addresses.Queries.GetCities;

public record GetCitiesQuery(int? CityId, QueryParam Param) : IRequest<PageMetaData<CityDto>>;
