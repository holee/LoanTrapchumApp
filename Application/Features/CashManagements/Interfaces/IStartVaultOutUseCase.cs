using Application.Features.Transactions.CashManagements;
using Application.Responses;

namespace Application.Features.CashManagements.Interfaces
{
    public interface IStartVaultOutUseCase
    {
        Task<Result<bool>> ExecuteAsync(TrnxCashVaultDtoCreate transaction);
    }
}