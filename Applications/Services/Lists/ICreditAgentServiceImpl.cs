using Applications.Requests.Loans.CreditAgents;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Loans;

namespace Applications.Services.Lists
{
    public interface ICreditAgentServiceImpl
    {
        Task<Result<string>> ActivateOrDeactivateCreditAgent(LoanCreditAgentDtoActivate req);
        Task<Result<string>> AddCreditAgent(string? employeeRefId, LoanCreditAgentDtoCreate req);
        Task<Result<string>> UpdateCreditAgent(string id, LoanCreditAgentDtoUpdate req);
        Task<Result<IEnumerable<VCreditAgent>>> GetCreditAgents(bool includeInactive = false, string? query = null);
        Task<Pager<VCreditAgent>> GetCreditAgents(QueryParam param);
        Task<Result<IEnumerable<VEmployeeCreateCreditAgent>>> GetEmployeeCreateCreditAgents(string? query = null);
        Task<Pager<VCreditAgent>> GetCreditAgents(int? branchId, QueryParam param);
    }
}
