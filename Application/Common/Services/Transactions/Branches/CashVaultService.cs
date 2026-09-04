using Application.Interfaces.Transactions;
using Application.Responses;
using Domains.Models.Transaction;
using Mapster;
using Application.Features.Transactions.CashManagements;
using Application.Features.Transactions.LoanTransactions;
using Application.Features.Currencies.Responses;
using Application.Features.CashManagements.Responses;

namespace Application.Services.Transactions.Branches
{

    public sealed class CashVaultService(ICashVaultService cService) : ICashVaultServiceImpl
    {
        public async Task<Result> CreateCashManagementAsync(LoanPaymentTransactionDtoCreate cashTrsansaction)
        {

            var req = cashTrsansaction.Adapt<LoanTransaction>();

            return await cService.CreateCashManagementAsync(req);
        }

        public async Task<Result> CreateCashVaultAsync(LoanPaymentTransactionDtoCreate transaction)
        {
            var req = transaction.Adapt<LoanTransaction>();

            return await cService.CreateCashVaultAsync(req);
        }

        public async Task<Result<bool>> CreateCashVaultAsync(TrnxCashVaultDtoCreate transaction)
        {
            var req = transaction.Adapt<LoanTransaction>();

            var rsp = await cService.CreateCashVaultAsync(req);

            return rsp.IsSuccess ? Result.Success(true) : Result.Failure<bool>(ErrorResponse.ServerError(rsp.ErrMessage));
        }

        public Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VCurrencyNotes>> GetCurrencyNotesAsync(int? branchId)
        {
            return await cService.GetCurrencyNotesAsync(branchId);
        }

        public async Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync()
        {
            return await cService.GetViewCashVaultAsync();
        }
    }
}
