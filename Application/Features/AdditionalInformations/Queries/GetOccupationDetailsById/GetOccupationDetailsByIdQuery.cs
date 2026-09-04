using Application.Features.AdditionalInformations.Dto;
using Domains.Shared.Enums;
using ErrorOr;

namespace Application.Features.AdditionalInformations.Queries.GetOccupationDetailsById;

public sealed record GetOccupationDetailsByIdQuery(int? Id)
    : IRequest<ErrorOr<OccupationDto>>;
