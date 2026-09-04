using Application.Features.CreditAgents.Requests;
using Application.Responses;
using Application.Responses.Paginations;
using Application.Responses.ViewModels.Customers;
using Application.Responses.ViewModels.Employees;

namespace Application.Services.Lists
{
    public interface ICreditAgentServiceImpl
    { 
        Task<Result<string>> ActivateOrDeactivateCreditAgent(LoanCreditAgentDtoActivate req);
        Task<Result<string>> AddCreditAgent(string? employeeRefId, LoanCreditAgentDtoCreate req);
        Task<Result<string>> UpdateCreditAgent(string id, LoanCreditAgentDtoUpdate req);
        Task<Result<IEnumerable<VCreditAgent>>> GetCreditAgents(bool includeInactive = false, string? query = null);
        Task<Result<IEnumerable<VEmployeeCreditAgent>>> GetEmployeeCreateCreditAgents(string? query = null);
        Task<Pager<VCreditAgent>> GetCreditAgents(int? branchId, QueryParam param);
        Task<Pager<VEmployeeCreditAgent>> GetEmployeeCreditAgents(int? branchId, string? employeeId, QueryParam param);
        Task<Result<string>> ChangeCreditAgents(int? branchId, List<LoanChangedCaDtoCreate> req);
    }
}
