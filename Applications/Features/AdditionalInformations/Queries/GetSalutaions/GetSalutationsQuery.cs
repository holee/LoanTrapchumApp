using Applications.Features.AdditionalInformations.Dto;

namespace Applications.Features.AdditionalInformations.Queries.GetSalutaions;

    public sealed record GetSalutationsQuery(int? Id,QueryParam Param) : IRequest<Pager<SalutationDto>>;