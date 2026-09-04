using Application.Features.AdditionalInformations.Dto;
using Application.Interfaces.Lists;

namespace Application.Features.AdditionalInformations.Queries.GetNationalities;

public sealed class GetNationalitiesQueryHandler(INationalityService nationalityService) : IRequestHandler<GetNationalitiesQuery, Pager<NationalityDto>>
{
    public async Task<Pager<NationalityDto>> Handle(GetNationalitiesQuery request, CancellationToken cancellationToken)
    {

        var param = request.Param ?? new QueryParam();

        var result = await nationalityService.GetNationalities(request.Id, param, cancellationToken);

        return result;
    }
}