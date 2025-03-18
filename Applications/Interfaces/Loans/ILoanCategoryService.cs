using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Categories;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanCategoryService
    {
        Task<Result<string>> Create(LoanCategory category);
        Task<Result<string>> Delete(int id);
        Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null);
        Task<IEnumerable<VlCategoryList>> GetListCategories(bool includeinactive=false,string? q = null);
        Task<PaginatedList<VlCategoryList>> GetAllPaginAsync(QueryParam param, bool isActive = false, string? q = null);
        Task<LoanCategory?> GetLoanCategory(int id);
        Task<Result<string>> Update(LoanCategory cat);
        Task<Result<string>> UpdateStatus(int id);
        Task<bool> NameExist(string? name);
        Task<bool> CodeExist(string? code);
    }
}
