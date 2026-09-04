using Application.Commons;
using Application.Features.Addresses.Dto;

namespace Application.Features.Addresses.Queries.GetCities;

public record GetCitiesQuery(int? CityId, QueryParam Param) : IRequest<PageMetaData<CityDto>>;
