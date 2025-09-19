using Applications.Enumerations;
using Applications.Features.CashManagements.Interfaces;
using Applications.Features.Transactions.CashManagements;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Domains.Models.Transaction;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.CashManagements.UseCases
{
    public sealed class StartVaultInUseCase : IStartVaultInUseCase
    {
        private readonly ICashVaultService _cashService;

        public StartVaultInUseCase(ICashVaultService cashVaultService)
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

            req.TranTypeRefId = (int)TransactionTypes.CSV;

            var rsp = await _cashService.CreateCashVaultAsync(req);

            return rsp.IsSuccess ? Result.Success(true) : Result.Failure<bool>(ErrorResponse.ServerError(rsp.ErrMessage));
        }
    }
}
