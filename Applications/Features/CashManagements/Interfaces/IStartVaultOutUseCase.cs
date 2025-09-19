using Applications.Features.Transactions.CashManagements;
using Applications.Responses;

namespace Applications.Features.CashManagements.Interfaces
{
    public interface IStartVaultOutUseCase
    {
        Task<Result<bool>> ExecuteAsync(TrnxCashVaultDtoCreate transaction);
    }
}