using Applications.Features.AdditionalInformations.Dto;
using Applications.Interfaces.Lists;

namespace Applications.Features.AdditionalInformations.Queries.GetNationalities;

public sealed class GetNationalitiesQueryHandler(INationalityService nationalityService) : IRequestHandler<GetNationalitiesQuery, Pager<NationalityDto>>
{
    public async Task<Pager<NationalityDto>> Handle(GetNationalitiesQuery request, CancellationToken cancellationToken)
    {

        var param = request.Param ?? new QueryParam();

        var result = await nationalityService.GetNationalities(request.Id, param, cancellationToken);

        return result;
    }
}