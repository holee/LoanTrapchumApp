using Applications.Responses.Paginations;
using Domains.Models.Loans;

namespace Applications.Interfaces.Loans
{
    public interface ILoanRequestImpl
    {
        Task<bool> CreateLoanRequest(LoanDisbursment request);
        Task<bool> EditLoanRequest(LoanDisbursment request);
        Task<LoanDisbursment?> GetLoanRequest(long id);
        Task<LoanDisbursment?> GetLoanRequestForUpdate(long id);
        Task<PaginatedList<LoanDisbursment>> GetLoanRequests(QueryParam param);
    }
}
