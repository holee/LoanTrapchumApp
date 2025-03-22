using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Loans;

namespace Applications.Services.Lists
{
    public interface ICreditAgentServiceImpl
    {
        Task<Result> ActivateOrDeactivateCreditAgent(LoanCreditAgent req);
        Task<Result> AddCreditAgent(long employeeRefId, LoanCreditAgent req);
        Task<IEnumerable<VCreditAgent>> GetCreditAgents(bool includeInactive = false, string? query = null);
        Task<Pager<VCreditAgent>> GetCreditAgents(int pageIndex, int pageSize, string sortColumn, bool includeIsRemove = false, bool sortDescending = false, string? query = null);
        Task<IEnumerable<VEmployeeCreateCreditAgent>> GetEmployeeCreateCreditAgents(string? query = null);
        Task<Result> UpdateCreditAgent(int id, LoanCreditAgent req);
    }
}
