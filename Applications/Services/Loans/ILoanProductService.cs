using Applications.Interfaces.Loans;
using Applications.Requests.Loans.Products;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Products;
using Domains.Models.Loans;
using Mapster;

namespace Applications.Services.Loans
{
    public interface ILoanProductServiceImpl
    {
        Task<Result<string>> CreateAsync(LoanProductDtoCreate model);
        Task<Result<string>> UpdateAsync(LoanProductDtoUpdate model);
        Task<Result<string>> UpdateAsync(int? id, bool state);
        Task<Result<string>> DeleteAsync(int id);
        Task<Result<IEnumerable<LoanProduct>>> GetAllAsyc(string? searches = null);
        Task<Pager<T>> GetLoanProductsAsync<T>(QueryParam param) where T : class;
        Task<Pager<VlProductList>> GetLoanProductsAsync(QueryParam param);
        Task<Result<LoanProduct>> GetByFieldAsync(string? fields = null);
        Task<Result<LoanProduct>> GetByIdAsync(int? id);
       
        Task<Result<T>> GetByIdAsync<T>(int? id) where T : class;
    }


    public sealed class LoanProductService(ILoanProductService pService) : ILoanProductServiceImpl
    {
        public async Task<Result<string>> CreateAsync(LoanProductDtoCreate model)
        {

            var req = model.Adapt<LoanProduct>();


            return await pService.CreateAsync(req);
        }

        public async Task<Result<string>> UpdateAsync(LoanProductDtoUpdate model)
        {
            var req = model.Adapt<LoanProduct>();

            return await pService.UpdateAsync(req);
        }

        public async Task<Result<string>> UpdateAsync(int? id, bool state)
        {
            return await pService.UpdateAsync(id, state);
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            return await pService.DeleteAsync(id);
        }

        public async Task<Result<IEnumerable<LoanProduct>>> GetAllAsyc(string? searches = null)
        {
            return await pService.GetAllAsyc(searches);
        }

        public async Task<Pager<T>> GetLoanProductsAsync<T>(QueryParam param)
            where T : class
        {
            return await pService.GetLoanProductsAsync<T>(param);
        }

        public async Task<Pager<VlProductList>> GetLoanProductsAsync(QueryParam param)
        {
            return await pService.GetLoanProductsAsync(param);
        }

        public async Task<Result<LoanProduct>> GetByFieldAsync(string? fields = null)
        {
            return await pService.GetByFieldAsync(fields);
        }

        public async Task<Result<LoanProduct>> GetByIdAsync(int? id)
        {
            return await pService.GetByIdAsync(id);
        }

        public async Task<Result<T>> GetByIdAsync<T>(int? id) where T : class
        {
            return await pService.GetByIdAsync<T>(id);
        }

    }
}
