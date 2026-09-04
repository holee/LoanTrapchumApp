using Application.Features.Collaterals.Dtos;
using Application.Interfaces.Lists;
using Application.Responses.Paginations;
using Application.Utils;

namespace Application.Features.Collaterals.Queries.GetCollateralAsPaging;

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
