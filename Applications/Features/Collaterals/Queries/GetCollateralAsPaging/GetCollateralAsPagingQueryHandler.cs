using Applications.Features.Collaterals.Dtos;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Collaterals.Queries.GetCollateralAsPaging;

public sealed class GetCollateralAsPagingQueryHandler : IRequestHandler<GetCollateralAsPagingQuery, Pager<VCollateral>>
{
    private readonly ICollateralsService _collaterals;

    public GetCollateralAsPagingQueryHandler(ICollateralsService collaterals)
    {
        _collaterals = collaterals;
    }

    public async Task<Pager<VCollateral>> Handle(GetCollateralAsPagingQuery request, CancellationToken cancellationToken = default)
    {
        return await _collaterals.GetCollateralsAysnc(request.Param, cancellationToken);
    }
}
