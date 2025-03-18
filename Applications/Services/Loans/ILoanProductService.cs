using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Products;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public sealed class LoanProductService(ILoanProductService pService) : ILoanProductService
    {
        public async Task<Result<string>> CreateAsync(LoanProduct model)
        {
            return await pService.CreateAsync(model);
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

        public async Task<Result<string>> ModifyAsync(LoanProduct model)
        {
            return await pService.ModifyAsync(model);
        }
    }
}
