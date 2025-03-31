using Applications.Interfaces.Lists;
using Applications.Requests.Loans.CreditAgents;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Customers;
using Applications.Responses.ViewModels.Employees;
using Domains.Models.Loans;
using Mapster;

namespace Applications.Services.Lists
{

    public sealed class CreditAgentService(ICreditAgentService crService) : ICreditAgentServiceImpl
    {
        public async Task<Result<string>> ActivateOrDeactivateCreditAgent(LoanCreditAgentDtoActivate req)
        {

            var model = req.Adapt<LoanCreditAgent>();

            return await crService.ActivateOrDeactivateCreditAgent(model);
        }

        public async Task<Result> AddCreditAgent(long employeeRefId, LoanCreditAgent req)
        {
            return await crService.AddCreditAgent(employeeRefId, req);
        }

        public Task<Result<string>> AddCreditAgent(Guid? employeeRefId, LoanCreditAgentDtoCreate req)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VCreditAgent>> GetCreditAgents(bool includeInactive = false, string? query = null)
        {
            return await crService.GetCreditAgents(includeInactive, query);
        }

        public async Task<Pager<VCreditAgent>> GetCreditAgents(int pageIndex, int pageSize, string sortColumn, bool includeIsRemove = false, bool sortDescending = false, string? query = null)
        {
            return await crService.GetCreditAgents(pageIndex, pageSize, sortColumn, includeIsRemove, sortDescending, query);
        }

        public async Task<IEnumerable<VEmployeeCreateCreditAgent>> GetEmployeeCreateCreditAgents(string? query = null)
        {
            return await crService.GetEmployeeCreateCreditAgents(query);
        }

        public async Task<Result> UpdateCreditAgent(int id, LoanCreditAgent req)
        {
            return await crService.UpdateCreditAgent(id, req);
        }

        public Task<Result<string>> UpdateCreditAgent(Guid id, LoanCreditAgentDtoUpdate req)
        {
            throw new NotImplementedException();
        }
    }
}
