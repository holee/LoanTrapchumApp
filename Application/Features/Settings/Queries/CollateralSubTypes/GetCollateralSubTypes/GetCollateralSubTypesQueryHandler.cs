using Application.Features.Settings.Dto;
using Application.Interfaces.Settings;
using Application.Responses.Paginations;

namespace Application.Features.Settings.Queries.CollateralSubTypes.GetCollateralSubTypes;

public sealed class GetCollateralSubTypesQueryHandler(IAppSettingService repository)
    : IRequestHandler<GetCollateralSubTypesQuery, Pager<CollateralSubTypeDto>>
{
    public Task<Pager<CollateralSubTypeDto>> Handle(GetCollateralSubTypesQuery request, CancellationToken cancellationToken)
        => repository.GetCollateralSubTypesAsync(request.Param, cancellationToken);
}
