using Application.Features.CollateralValuations.Dtos;
using Application.Responses.Paginations;
using Domains.Models.Collaterals;
using ErrorOr;

namespace Application.Interfaces
{
    public interface ICollateralValuationService
    {
        Task<IEnumerable<CollateralValuationDto>> GetAllAsync(string? searchText = null, CancellationToken cancellationToken = default);
        Task<Pager<CollateralValuationDto>> GetAsPagingAsync(QueryParam param, CancellationToken cancellationToken = default);
        Task<ErrorOr<CollateralValuationDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ErrorOr<string>> CreateAsync(CollateralEvaluations entity, CancellationToken cancellationToken = default);
        Task<ErrorOr<string>> UpdateAsync(CollateralEvaluations entity, CancellationToken cancellationToken = default);
        Task<ErrorOr<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}
