using Applications.Responses.Auths;
using Applications.Responses.Loans.Disbursements;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanSettingService 
    {
        Task<IEnumerable<LoanInstallmentType>> GetInstallmentTypes(string? q = null);
        //Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null);
        Task<IEnumerable<LoanRepaymentMode>> GetRepaymentModes(string? q = null);
        Task<IEnumerable<LoanSubTypeStatus>> GetSubSubTypeStatus(int? subTypeRefId = null, string? q = null);
        Task<IEnumerable<LoanTypeStatus>> GetSubTypeStatus(string? q = null);
        Task<IEnumerable<VUserLoanSizes>> GetAprovedUserAsync(int? branchId, int? currencyId, decimal? amount, QueryParam param, CancellationToken cancellation = default);
        Task<Pager<LoanCategoryDto>> GetLoanCategories(int? id, QueryParam param, CancellationToken cancellation = default);
    }
}
