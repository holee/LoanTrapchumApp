using Applications.Features.CollateralValuations.Dtos;
using Applications.Interfaces;
using Applications.Responses.Paginations;

namespace Applications.Features.CollateralValuations.Queries.GetCollateralValuationsAsPaging
{
    public class GetCollateralValuationsAsPagingQuery
    {
        public QueryParam Param { get; set; } = new();
    }

    public class GetCollateralValuationsAsPagingQueryHandler(ICollateralValuationService service)
    {
        public async Task<Pager<CollateralValuationDto>> Handle(GetCollateralValuationsAsPagingQuery query, CancellationToken cancellationToken = default)
        {
            return await service.GetAsPagingAsync(query.Param, cancellationToken);
        }
    }
}
