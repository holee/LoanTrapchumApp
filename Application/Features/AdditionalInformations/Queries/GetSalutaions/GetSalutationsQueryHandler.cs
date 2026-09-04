using Application.Features.AdditionalInformations.Dto;
using Application.Interfaces.Lists;

namespace Application.Features.AdditionalInformations.Queries.GetSalutaions;

public sealed class GetSalutationsQueryHandler(ISalutationService salutationService)
    : IRequestHandler<GetSalutationsQuery, Pager<SalutationDto>>
{
    public async Task<Pager<SalutationDto>> Handle(GetSalutationsQuery request,
        CancellationToken cancellationToken = default)
    {
        var salu =await salutationService.GetsSalutions(request.Id,request.Param,cancellationToken);
            
        return salu;
            
    }
}