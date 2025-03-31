using Applications.Interfaces.Loans;
using Applications.Requests.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Categories;
using Domains.Models.Loans;
using Mapster;

namespace Applications.Services.Loans
{
    public interface ILoanCategoryServiceImpl
    {
        Task<bool> CodeExist(string? code);
        Task<Result<string>> CreateAsync(LoanCategoryDtoCreate category);
        Task<Result<string>> UpdateAsync(LoanCategoryDtoUpdate cat);
        Task<Result<string>> DeleteAsync(int? id);
        Task<PaginatedList<VlCategoryList>> GetAllPaginAsync(QueryParam param, bool isActive = false, string? q = null);
        Task<LoanCategoryDtoUpdate?> GetForEditAsync(int? id);
        Task<IEnumerable<VlCategoryList>> GetListCategories(bool includeinactive = false, string? q = null);
        Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null);
        Task<LoanCategory?> GetLoanCategory(int? id);
        Task<bool> NameExist(string? name);

        Task<Result<string>> UpdateStatus(int id);
    }


    public sealed class LoanCategoryService(ILoanCategoryService cService) : ILoanCategoryServiceImpl
    {
        public async Task<bool> CodeExist(string? code)
        {
            return await cService.CodeExist(code);
        }

        public async Task<Result<string>> CreateAsync(LoanCategoryDtoCreate req)
        {

            var category = req.Adapt<LoanCategory>();

            return await cService.Create(category);
        }


        public async Task<Result<string>> UpdateAsync(LoanCategoryDtoUpdate req)
        {
            var category = req.Adapt<LoanCategory>();

            return await cService.Update(category);
        }
        public async Task<Result<string>> DeleteAsync(int? id)
        {
            return await cService.Delete(id!.Value);
        }

        public async Task<PaginatedList<VlCategoryList>> GetAllPaginAsync(QueryParam param, bool isActive = false, string? q = null)
        {
            return await cService.GetAllPaginAsync(param, isActive, q);
        }

        public Task<LoanCategoryDtoUpdate?> GetForEditAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VlCategoryList>> GetListCategories(bool includeinactive = false, string? q = null)
        {
            return await cService.GetListCategories(includeinactive, q);
        }

        public async Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null)
        {
            return await cService.GetLoanCategories(q);
        }

        public async Task<LoanCategory?> GetLoanCategory(int? id)
        {
            return await cService.GetLoanCategory(id!.Value);
        }

        public async Task<bool> NameExist(string? name)
        {
            return await cService.NameExist(name);
        }



        public async Task<Result<string>> UpdateStatus(int id)
        {
            return await cService.UpdateStatus(id);
        }
    }
}
