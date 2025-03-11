using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;

namespace Applications.Interfaces.Transactions
{
    public interface ICashVaultServiceImpl
    {
        Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync();
        Task<IEnumerable<ViewModelCurrencyNotes>> GetCurrencyNotesAsync(int branchId);
        Task<Result> CreateCashManagementAsync(LoanTransaction cashTrsansaction); 
        Task<Result> CreateCashVaultAsync(LoanTransaction transaction);
    }
}
