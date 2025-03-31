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
        Task<Result<string>> AddCreditAgent(Guid? employeeRefId, LoanCreditAgentDtoCreate req);
        Task<Result<string>> UpdateCreditAgent(Guid id, LoanCreditAgentDtoUpdate req);
        Task<IEnumerable<VCreditAgent>> GetCreditAgents(bool includeInactive = false, string? query = null);
        Task<Pager<VCreditAgent>> GetCreditAgents(int pageIndex, int pageSize, string sortColumn, bool includeIsRemove = false, bool sortDescending = false, string? query = null);
        Task<IEnumerable<VEmployeeCreateCreditAgent>> GetEmployeeCreateCreditAgents(string? query = null);
       
    }
}
