using Applications.Features.AdditionalInformations.Dto;

namespace Applications.Features.AdditionalInformations.Queries.GetNationalities;

public record GetNationalitiesQuery(int? Id, QueryParam Param) : IRequest<Pager<NationalityDto>>;
