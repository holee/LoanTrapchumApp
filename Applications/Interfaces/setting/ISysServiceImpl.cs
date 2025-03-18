using Domains.Models.Collaterals;
using Domains.Models.Loans;
using Domains.Models.Transaction;

namespace Applications.Interfaces.setting
{
    public interface ISysService 
    {
        Task<IEnumerable<CollateralType>> GetCollateralTypes(string? q = null);
        Task<IEnumerable<LoanTermType>> GetLoanTerms(string? q = null);
        Task<IEnumerable<LoanPaymentLocation>> GetPaymentLocations(string? q = null);
        Task<IEnumerable<LoanStatus>> GetLoanStatuses(string? q = null);
        Task<IEnumerable<LoanScheduleStatus>> GetLoanScheduleStatuses(string? q = null);
        // Cash Status
        Task<IEnumerable<CashStatus>> GetCashStatusAsync(string? q = null);
    }
}
