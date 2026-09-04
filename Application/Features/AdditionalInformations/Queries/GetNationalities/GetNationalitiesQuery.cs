using Application.Features.AdditionalInformations.Dto;

namespace Application.Features.AdditionalInformations.Queries.GetNationalities;

public record GetNationalitiesQuery(int? Id, QueryParam? Param) : IRequest<Pager<NationalityDto>>;
