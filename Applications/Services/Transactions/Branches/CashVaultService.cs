using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Services.Transactions.Branches
{
    public sealed class CashVaultService(ICashVaultService cService) : ICashVaultService
    {
        public async Task<Result> CreateCashManagementAsync(LoanTransaction cashTrsansaction)
        {
            return await cService.CreateCashManagementAsync(cashTrsansaction);
        }

        public async Task<Result> CreateCashVaultAsync(LoanTransaction transaction)
        {
            return await cService.CreateCashVaultAsync(transaction);
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
