using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Categories;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Categories;
using Domains.Models.Loans;
using Mapster;

namespace Applications.Services.Loans
{
    public interface ILoanCategoryServiceImpl
    {
        Task<Result<string>> CreateAsync(LoanCategoryDtoCreate category);
        Task<Result<string>> UpdateAsync(LoanCategoryDtoUpdate cat);
        Task<Result<string>> DeleteAsync(int? id);
        Task<Pager<VlCategoryList>> GetCategoriesAsync(QueryParam param);
        Task<Result<LoanCategoryDtoUpdate>> GetForEditAsync(int? id);
        Task<Pager<LoanCategory>> GetCategoriesAsync(string? q = null);
        Task<Result<LoanCategory>> GetLoanCategory(int? id);
        Task<Result<string>> UpdateStatus(int id);
    }


    public sealed class LoanCategoryService(ILoanCategoryService cService) : ILoanCategoryServiceImpl
    {

        public async Task<Result<string>> CreateAsync(LoanCategoryDtoCreate req)
        {

            var category = req.Adapt<LoanCategory>();

            return await cService.CreateAsync(category);
        }


        public async Task<Result<string>> UpdateAsync(LoanCategoryDtoUpdate req)
        {
            var category = req.Adapt<LoanCategory>();

            return await cService.UpdateAsync(category);
        }
        public async Task<Result<string>> DeleteAsync(int? id)
        {
            return await cService.DeleteAsync(id!.Value);
        }

        public async Task<Pager<VlCategoryList>> GetCategoriesAsync(QueryParam param)
        {
            return await cService.GetCategoriesAsync(param);
        }


        public async Task<Pager<LoanCategory>> GetCategoriesAsync(string? q = null)
        {
            return await cService.GetCategoriesAsync(q);
        }

        public async Task<Result<LoanCategory>> GetLoanCategory(int? id)
        {
            return await cService.GetLoanCategoryByIdAsync(id);
        }

        public async Task<Result<string>> UpdateStatus(int? id)
        {
            return await cService.UpdateStatus(id);
        }

        public async Task<Result<LoanCategoryDtoUpdate>> GetForEditAsync(int? id)
        {
            return  await cService.GetEditCategoryByIdAsync(id);
        }

        public async Task<Result<string>> UpdateStatus(int id)
        {
            return await cService.UpdateStatus(id);
        }
    }
}
