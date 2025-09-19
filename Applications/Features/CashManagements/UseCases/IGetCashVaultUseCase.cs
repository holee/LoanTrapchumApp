using Applications.Features.CashManagements.Responses;
using Applications.Responses;

namespace Applications.Features.CashManagements.UseCases
{
    public interface IGetCashVaultUseCase
    {
        Task<Result<IEnumerable<VCashVaults>>> ExecuteAsync(int? branchId);
    }
}