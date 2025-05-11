using Applications.Interfaces.Transactions;
using Applications.Requests.Transactions.LoanTransactions;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applications.Requests.Transactions.CashManagements;

namespace Applications.Services.Transactions.Branches
{

    public sealed class CashVaultService(ICashVaultService cService) : ICashVaultServiceImpl
    {
        public async Task<Result> CreateCashManagementAsync(LoanTransactionDtoCreate cashTrsansaction)
        {

            var req = cashTrsansaction.Adapt<LoanTransaction>();

            return await cService.CreateCashManagementAsync(req);
        }

        public async Task<Result> CreateCashVaultAsync(LoanTransactionDtoCreate transaction)
        {
            var req = transaction.Adapt<LoanTransaction>();

            return await cService.CreateCashVaultAsync(req);
        }

        public Task<Result<string>> CreateCashVaultAsync(TransactionCashVaultDtoCreate transaction)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ViewModelCurrencyNotes>> GetCurrencyNotesAsync(int? branchId)
        {
            return await cService.GetCurrencyNotesAsync(branchId);
        }

        public async Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync()
        {
            return await cService.GetViewCashVaultAsync();
        }
    }
}
