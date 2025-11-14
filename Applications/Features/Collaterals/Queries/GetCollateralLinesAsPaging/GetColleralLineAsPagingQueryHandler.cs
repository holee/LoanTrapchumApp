using Applications.Features.Collaterals.Dtos;
using Applications.Interfaces.Customers;
using Applications.Responses.Paginations;
using Applications.Utils;

namespace Applications.Features.Collaterals.Queries.GetCollateralLinesAsPaging;

public sealed class GetColleralLineAsPagingQueryHandler : IRequestHandler<GetCollateralLineAsPagingQuery, Pager<VCollateralLines>>
{
    private readonly ICollateralService _collateralService;
    public GetColleralLineAsPagingQueryHandler(ICollateralService collateralService)
    {
        _collateralService = collateralService;
    }
    public async Task<Pager<VCollateralLines>> Handle(GetCollateralLineAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await _collateralService.GetCollateralLinesAsPaging(request.CustomerId, request.Query, cancellationToken);
    }
}