using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Products;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public interface ILoanProductService
    {
        Task<Result<IEnumerable<LoanProduct>>> GetAllAsyc(string? searches=null);
        Task<Result<IEnumerable<VlProductList>>> GetAllListAsyc(bool includeinactive=false,string? searches=null);
        Task<PaginatedList<VlProductList>> GetAllPagingAsyc(QueryParam param, bool isActive = false, string? searches = null);
        Task<Result<LoanProduct>> GetByIdAsync(int id);
        Task<Result<LoanProduct>> GetByFieldAsync(string? fields = null);
        Task<Result<string>> CreateAsync(LoanProduct model);
        Task<Result<string>> ModifyAsync(LoanProduct model);
        Task<Result<string>> DeleteAsync(int id);
        Task SaveAsync();
    }
}
