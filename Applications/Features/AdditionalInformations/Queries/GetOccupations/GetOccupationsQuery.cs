using Applications.Features.AdditionalInformations.Dto;
using Domains.Shared.Enums;

namespace Applications.Features.AdditionalInformations.Queries.GetOccupations;

public sealed record GetOccupationsQuery(int? Id, QueryParam? Param, TypeOfEnum Type = TypeOfEnum.Position)
    : IRequest<Pager<OccupationDto>>;
