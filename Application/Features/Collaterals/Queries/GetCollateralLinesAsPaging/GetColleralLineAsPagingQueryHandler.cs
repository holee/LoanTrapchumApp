using Application.Features.Collaterals.Dtos;
using Application.Interfaces.Customers;
using Application.Responses.Paginations;
using Application.Utils;

namespace Application.Features.Collaterals.Queries.GetCollateralLinesAsPaging;

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