using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.CashManagements;
using Applications.Features.Transactions.CashManagements;
using Applications.Features.Transactions.LoanTransactions;
using Applications.Responses;

namespace Applications.Services.Transactions.Branches
{
    public interface ICashVaultServiceImpl
    {
        Task<Result> CreateCashManagementAsync(LoanPaymentTransactionDtoCreate cashTrsansaction);
        Task<Result> CreateCashVaultAsync(LoanPaymentTransactionDtoCreate transaction);
        
        //TransactionCashVaultDtoCreate
        Task<Result<bool>> CreateCashVaultAsync(TrnxCashVaultDtoCreate transaction);
        Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId);
        Task<IEnumerable<VCurrencyNotes>> GetCurrencyNotesAsync(int? branchId);
        Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync();
    }
}
