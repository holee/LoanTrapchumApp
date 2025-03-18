using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Categories;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public sealed class LoanCategoryService(ILoanCategoryService cService) : ILoanCategoryService
    {
        public async Task<bool> CodeExist(string? code)
        {
            return await cService.CodeExist(code);
        }

        public async Task<Result<string>> Create(LoanCategory category)
        {
           return await cService.Create(category);
        }

        public async Task<Result<string>> Delete(int id)
        {
           return await cService.Delete(id);
        }

        public async Task<PaginatedList<VlCategoryList>> GetAllPaginAsync(QueryParam param, bool isActive = false, string? q = null)
        {
            return await cService.GetAllPaginAsync(param, isActive, q);
        }

        public async Task<IEnumerable<VlCategoryList>> GetListCategories(bool includeinactive = false, string? q = null)
        {
            return await cService.GetListCategories(includeinactive, q);
        }

        public async Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null)
        {
            return await cService.GetLoanCategories(q);
        }

        public async Task<LoanCategory?> GetLoanCategory(int id)
        {
            return await cService.GetLoanCategory(id);
        }

        public async Task<bool> NameExist(string? name)
        {
            return await cService.NameExist(name);
        }

        public async Task<Result<string>> Update(LoanCategory cat)
        {
            return await cService.Update(cat);
        }

        public async Task<Result<string>> UpdateStatus(int id)
        {
            return await cService.UpdateStatus(id);
        }
    }
}
