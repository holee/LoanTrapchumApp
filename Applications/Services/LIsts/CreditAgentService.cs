using Applications.Interfaces.Lists;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Loans;

namespace Applications.Services.Lists
{

    public sealed class CreditAgentService : ICreditAgentServiceImpl
    {
        private readonly ICreditAgentService _crService;

        public CreditAgentService(ICreditAgentService crService)
        {
            _crService = crService;
        }

        public async Task<Result> ActivateOrDeactivateCreditAgent(LoanCreditAgent req)
        {
            return await _crService.ActivateOrDeactivateCreditAgent(req);
        }

        public async Task<Result> AddCreditAgent(long employeeRefId, LoanCreditAgent req)
        {
            return await _crService.AddCreditAgent(employeeRefId, req);
        }

        public async Task<IEnumerable<VCreditAgent>> GetCreditAgents(bool includeInactive = false, string? query = null)
        {
            return await _crService.GetCreditAgents(includeInactive, query);
        }

        public async Task<Pager<VCreditAgent>> GetCreditAgents(int pageIndex, int pageSize, string sortColumn, bool includeIsRemove = false, bool sortDescending = false, string? query = null)
        {
            return await _crService.GetCreditAgents(pageIndex, pageSize, sortColumn, includeIsRemove, sortDescending, query);
        }

        public async Task<IEnumerable<VEmployeeCreateCreditAgent>> GetEmployeeCreateCreditAgents(string? query = null)
        {
            return await _crService.GetEmployeeCreateCreditAgents(query);
        }

        public async Task<Result> UpdateCreditAgent(int id, LoanCreditAgent req)
        {
            return await _crService.UpdateCreditAgent(id, req);
        }
    
    }
}
