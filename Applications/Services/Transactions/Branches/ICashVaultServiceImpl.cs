using Applications.Requests.Transactions.CashManagements;
using Applications.Requests.Transactions.LoanTransactions;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;

namespace Applications.Services.Transactions.Branches
{
    public interface ICashVaultServiceImpl
    {
        Task<Result> CreateCashManagementAsync(LoanTransactionDtoCreate cashTrsansaction);
        Task<Result> CreateCashVaultAsync(LoanTransactionDtoCreate transaction);
        
        //TransactionCashVaultDtoCreate
        Task<Result<string>> CreateCashVaultAsync(TransactionCashVaultDtoCreate transaction);
        Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId);
        Task<IEnumerable<ViewModelCurrencyNotes>> GetCurrencyNotesAsync(int? branchId);
        Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync();
    }
}
