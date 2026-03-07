using Applications.Features.Transactions.CashManagements;
using Applications.Features.Transactions.LoanTransactions;

namespace Applications.Services.Transactions.Branches
{
    public interface ICashVaultServiceImpl
    {
        Task<Result> CreateCashManagementAsync(LoanPaymentTransactionDtoCreate cashTrsansaction);
        Task<Result> CreateCashVaultAsync(LoanPaymentTransactionDtoCreate transaction);
        Task<Result<bool>> CreateCashVaultAsync(TrnxCashVaultDtoCreate transaction);
        Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId);
        Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync();
        Task<IEnumerable<VCurrencyNotes>> GetCurrencyNotesAsync(int? branchId);
    }
}
