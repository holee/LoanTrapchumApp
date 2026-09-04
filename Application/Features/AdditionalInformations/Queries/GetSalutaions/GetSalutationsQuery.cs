using Application.Features.AdditionalInformations.Dto;

namespace Application.Features.AdditionalInformations.Queries.GetSalutaions;

    public sealed record GetSalutationsQuery(int? Id,QueryParam Param) : IRequest<Pager<SalutationDto>>;