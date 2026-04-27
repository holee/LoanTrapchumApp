using Applications.Features.AdditionalInformations.Dto;
using Applications.Interfaces.Lists;

namespace Applications.Features.AdditionalInformations.Queries.GetOccupations;

public sealed record GetOccupationsQueryHandler : IRequestHandler<GetOccupationsQuery, Pager<OccupationDto>>
{
    private readonly IOccupationService _occupationService;
    public GetOccupationsQueryHandler(IOccupationService occupationService)
    {
        _occupationService = occupationService;
    }
    public async Task<Pager<OccupationDto>> Handle(GetOccupationsQuery request, CancellationToken cancellationToken)
    {
        var param = request.Param ?? new QueryParam();

        var rsp = await _occupationService.GetOccupationsAsync(request.Id, param, request.Type, cancellationToken);

        return rsp;
    }
}
