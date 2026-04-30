using Applications.Features.CollateralValuations.Dtos;
using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.CollateralValuations.Queries.GetCollateralValuationById
{
    public class GetCollateralValuationByIdQuery
    {
        public long Id { get; set; }
    }

    public class GetCollateralValuationByIdQueryHandler(ICollateralValuationService service)
    {
        public async Task<ErrorOr<CollateralValuationDto>> Handle(GetCollateralValuationByIdQuery query, CancellationToken cancellationToken = default)
        {
            return await service.GetByIdAsync(query.Id, cancellationToken);
        }
    }
}
