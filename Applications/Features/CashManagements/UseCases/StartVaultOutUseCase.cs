using Applications.Features.CashManagements.Interfaces;
using Applications.Features.Transactions.CashManagements;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Domains.Models.Transaction;
using Mapster;

namespace Applications.Features.CashManagements.UseCases
{
    public sealed class StartVaultOutUseCase : IStartVaultOutUseCase
    {
        private readonly ICashVaultService _cashService;

        public StartVaultOutUseCase(ICashVaultService cashVaultService)
        {
            _cashService = cashVaultService;
        }

        public async Task<Result<bool>> ExecuteAsync(TrnxCashVaultDtoCreate trnx)
        {

            if (!trnx.CashManagements.Any())
            {
                return Result.Failure<bool>(ErrorResponse.Validation("Cannot process this transaction because of invalid cash."));
            }


            var cash = trnx.CashManagements.First();

            if (!cash.CashManagementDetails.Any())
            {
                return Result.Failure<bool>(ErrorResponse.Validation("Cannot process this transaction because of invalid cash details."));
            }

            var req = trnx.Adapt<LoanTransaction>();

            var rsp = await _cashService.CreateCashVaultAsync(req);

            return rsp.IsSuccess ? Result.Success(true) : Result.Failure<bool>(ErrorResponse.ServerError(rsp.ErrMessage));
        }
    }
}
