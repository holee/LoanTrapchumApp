using Application.Features.AdditionalInformations.Dto;
using Domains.Shared.Enums;

namespace Application.Features.AdditionalInformations.Queries.GetOccupations;

public sealed record GetOccupationsQuery(int? Id, QueryParam? Param, TypeOfEnum Type = TypeOfEnum.Position)
    : IRequest<Pager<OccupationDto>>;
