using Applications.Features.AdditionalInformations.Dto;
using Domains.Shared.Enums;
using ErrorOr;

namespace Applications.Features.AdditionalInformations.Queries.GetOccupationDetailsById;

public sealed record GetOccupationDetailsByIdQuery(int? Id)
    : IRequest<ErrorOr<OccupationDto>>;
