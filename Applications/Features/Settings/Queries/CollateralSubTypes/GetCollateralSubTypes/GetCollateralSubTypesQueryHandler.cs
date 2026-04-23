using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using Applications.Responses.Paginations;

namespace Applications.Features.Settings.Queries.CollateralSubTypes.GetCollateralSubTypes;

public sealed class GetCollateralSubTypesQueryHandler(IAppSettingRepository repository)
    : IRequestHandler<GetCollateralSubTypesQuery, Pager<CollateralSubTypeDto>>
{
    public Task<Pager<CollateralSubTypeDto>> Handle(GetCollateralSubTypesQuery request, CancellationToken cancellationToken)
        => repository.GetCollateralSubTypesAsync(request.Param, cancellationToken);
}
