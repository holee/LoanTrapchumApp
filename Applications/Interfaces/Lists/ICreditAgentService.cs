using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Loans;

namespace Applications.Interfaces.Lists
{
    public interface ICreditAgentService
    {
        Task<Result<string>> AddCreditAgent(string? employeeRefId, LoanCreditAgent req);
        Task<Result<string>> ActivateOrDeactivateCreditAgent(LoanCreditAgent req);
        Task<Result<IEnumerable<VCreditAgent>>> GetCreditAgents(bool includeInactive=false, string? query = null);
        Task<Result> UpdateCreditAgent(string? id, LoanCreditAgent req);
        Task<Result<IEnumerable<VEmployeeCreateCreditAgent>>> GetEmployeeCreateCreditAgents(string? query = null);
        Task<Pager<VCreditAgent>> GetCreditAgents(int? branchId, QueryParam param);
    }
}
