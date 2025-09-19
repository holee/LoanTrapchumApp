using Applications.Features.Categories.Requests;
using Applications.Features.Categories.Requests;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Categories;
using Domains.Models.Bases;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanCategoryService
    {
        Task<Result<string>> CreateAsync(LoanCategory category);
        Task<Result<string>> DeleteAsync(int? id);
        Task<Result<string>> UpdateAsync(LoanCategory cat);
        Task<Result<string>> UpdateStatus(int? id);
        Task<Pager<VlCategoryList>> GetCategoriesAsync(QueryParam param);
        Task<Result<LoanCategory>> GetLoanCategoryByIdAsync(int? id);
        Task<Result<VlCategoryList>> GetCategoryAsync(int? id);
        Task<Result<LoanCategoryDtoUpdate>> GetEditCategoryByIdAsync(int? id);
        Task<Result<string>> UpdateStatusAsync(int? id, ChangeState state, CancellationToken cancellation = default);
    }
}
