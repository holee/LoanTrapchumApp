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
        Task<Result<string>> DeleteAsync(int id);
        Task<Result<IEnumerable<LoanProduct>>> GetAllAsyc(string? searches = null);
        Task<Result<IEnumerable<VlProductList>>> GetAllListAsyc(bool includeinactive = false, string? searches = null);
        Task<PaginatedList<VlProductList>> GetAllPagingAsyc(QueryParam param, bool isActive = false, string? searches = null);
        Task<Result<LoanProduct>> GetByFieldAsync(string? fields = null);
        Task<Result<LoanProduct>> GetByIdAsync(int id);

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

            return await pService.ModifyAsync(req);
        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            return await pService.DeleteAsync(id);
        }

        public async Task<Result<IEnumerable<LoanProduct>>> GetAllAsyc(string? searches = null)
        {
            return await pService.GetAllAsyc(searches);
        }

        public async Task<Result<IEnumerable<VlProductList>>> GetAllListAsyc(bool includeinactive = false, string? searches = null)
        {
            return await pService.GetAllListAsyc(includeinactive, searches);
        }

        public async Task<PaginatedList<VlProductList>> GetAllPagingAsyc(QueryParam param, bool isActive = false, string? searches = null)
        {
            return await pService.GetAllPagingAsyc(param, isActive, searches);
        }

        public async Task<Result<LoanProduct>> GetByFieldAsync(string? fields = null)
        {
            return await pService.GetByFieldAsync(fields);
        }

        public async Task<Result<LoanProduct>> GetByIdAsync(int id)
        {
            return await pService.GetByIdAsync(id);
        }


    }
}
