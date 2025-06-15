using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Products;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanProductService
    {
        Task<Result<IEnumerable<LoanProduct>>> GetAllAsyc(string? searches=null);
        Task<Result<LoanProduct>> GetByIdAsync(int? id);
        Task<Result<LoanProduct>> GetByFieldAsync(string? fields = null);
        Task<Result<string>> CreateAsync(LoanProduct model);
        Task<Result<string>> UpdateAsync(LoanProduct model);
        //Task<Result<string>> UpdateAsync(int? id, bool state);
        Task<Result<string>> DeleteAsync(int? id);
        Task<Pager<VlProductList>> GetLoanProductsAsync(QueryParam param);
        Task<Pager<T>> GetLoanProductsAsync<T>(QueryParam param) where T : class;
        Task<Result<T>> GetByIdAsync<T>(int? id) where T : class;
        Task<Result<string>> UpdateAsync(int? id, bool state, CancellationToken cancellation = default);
    }
}
