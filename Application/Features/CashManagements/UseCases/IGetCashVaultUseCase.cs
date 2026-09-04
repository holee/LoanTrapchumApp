using Application.Features.CashManagements.Responses;
using Application.Responses;

namespace Application.Features.CashManagements.UseCases
{
    public interface IGetCashVaultUseCase
    {
        Task<Result<IEnumerable<VCashVaults>>> ExecuteAsync(int? branchId);
    }
}