using Application.Features.CollateralValuations.Dtos;
using Application.Interfaces;
using ErrorOr;

namespace Application.Features.CollateralValuations.Queries.GetCollateralValuationById
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
