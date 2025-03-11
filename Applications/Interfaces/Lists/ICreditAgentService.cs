using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Loans;

namespace Applications.Interfaces.Lists
{
    public interface ICreditAgentService
    {
        Task<Result> AddCreditAgent(long employeeRefId, LoanCreditAgent req);
        Task<Result> ActivateOrDeactivateCreditAgent(LoanCreditAgent req);
        Task<IEnumerable<VCreditAgent>> GetCreditAgents(bool includeInactive=false, string? query = null);
        Task<Result> UpdateCreditAgent(int id, LoanCreditAgent req);
        Task<IEnumerable<VEmployeeCreateCreditAgent>> GetEmployeeCreateCreditAgents(string? query = null);
        Task<Pager<VCreditAgent>> GetCreditAgents(int pageIndex, int pageSize, string sortColumn, bool includeIsRemove = false, bool sortDescending = false, string? query = null);
    }
}
