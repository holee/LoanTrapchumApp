using Applications.Responses.Paginations;
using Domains.Models.Loans;

namespace Applications.Services.Loans
{
    public interface ILoanRequest
    {
        Task<bool> CreateLoanRequest(Loan request);
        Task<bool> EditLoanRequest(Loan request);
        Task<Loan?> GetLoanRequest(long id);
        Task<Loan?> GetLoanRequestForUpdate(long id);
        Task<PaginatedList<Loan>> GetLoanRequests(QueryParam param);
    }
}
