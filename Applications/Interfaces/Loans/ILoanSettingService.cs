using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Settings;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanSettingService 
    {
        Task<IEnumerable<LoanInstallmentType>> GetInstallmentTypes(string? q = null);
        Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null);
        Task<IEnumerable<LoanRepaymentMode>> GetRepaymentModes(string? q = null);
        Task<IEnumerable<LoanSubTypeStatus>> GetSubSubTypeStatus(int? subTypeRefId = null, string? q = null);
        Task<IEnumerable<LoanTypeStatus>> GetSubTypeStatus(string? q = null);
        Task<IEnumerable<AprovePerson>> GetAprovedUserAsync(int? branchId, string? filter = null);
        Task<IEnumerable<T>> GetAprovedUserAsync<T>(int? branchId, string? filter = null) where T : class;
        Task<IEnumerable<VUserLoanSizes>> GetAprovedUserAsync(int? branchId, int? currencyId = 1, decimal? amount = 0, string? filter = null);
    }
}
