using Application.Features.Transactions.CashManagements;
using Application.Responses;

namespace Application.Features.CashManagements.Interfaces
{
    public interface IStartVaultInUseCase
    {
        Task<Result<bool>> ExecuteAsync(TrnxCashVaultDtoCreate transaction);
    }
}