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

            return await crService.ChangeCreditAgentState(model);
        }

        public async Task<Result<string>> AddCreditAgent(string? employeeRefId, LoanCreditAgentDtoCreate req)
        {

            var model = req.Adapt<LoanCreditAgent>();

            return await crService.AddCreditAgent(employeeRefId, model);
        }

        public async Task<Result<IEnumerable<VCreditAgent>>> GetCreditAgents(bool includeInactive = false, string? query = null)
        {
            return await crService.GetCreditAgents(includeInactive, query);
        }

        public async Task<Pager<VCreditAgent>> GetCreditAgents(QueryParam param)
        {
            return await crService.GetCreditAgents(0,param);
        }

        public async Task<Pager<VCreditAgent>> GetCreditAgents(int? branchId,QueryParam param)
        {
            return await crService.GetCreditAgents(branchId, param);
        }

        public async Task<Result<IEnumerable<VEmployeeCreateCreditAgent>>> GetEmployeeCreateCreditAgents(string? query = null)
        {
            return await crService.GetEmployeeCreateCreditAgents(query);
        }

        public async Task<Result> UpdateCreditAgent(string? id, LoanCreditAgent req)
        {
            return await crService.UpdateCreditAgent(id, req);
        }

        public Task<Result<string>> UpdateCreditAgent(string id, LoanCreditAgentDtoUpdate req)
        {
            throw new NotImplementedException();
        }
    }
}
