using Application.Responses;
using Application.Responses.Paginations;
using Application.Responses.ViewModels.Customers;
using Application.Responses.ViewModels.Employees;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans;

namespace Application.Interfaces.Lists
{
    public interface ICreditAgentService
    {
        Task<Result<string>> AddCreditAgent(string? employeeRefId, LoanCreditAgent req);
        Task<Result<string>> ChangeCreditAgentState(LoanCreditAgent req);
        Task<Result<IEnumerable<VCreditAgent>>> GetCreditAgents(bool includeInactive=false, string? query = null);
        Task<Result> UpdateCreditAgent(string? id, LoanCreditAgent req);
        Task<Result<IEnumerable<VEmployeeCreditAgent>>> GetEmployeeCreateCreditAgents(string? query = null);
        Task<Pager<VCreditAgent>> GetCreditAgents(int? branchId, QueryParam param, CancellationToken cancellation = default);
        Task<Pager<VEmployeeCreditAgent>> GetEmployeeCreditAgents(int? branchId, string? employeeId, QueryParam param, CancellationToken cancellation = default);
        Task<Result<string>> ChangedCreditAgents(int? branchId, List<LoanChangedCa> req, CancellationToken cancellation = default);
    }
}
