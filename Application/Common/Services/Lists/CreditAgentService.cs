using Application.Features.CreditAgents.Requests;
using Application.Interfaces.Lists;
using Application.Responses;
using Application.Responses.Paginations;
using Application.Responses.ViewModels.Customers;
using Application.Responses.ViewModels.Employees;
using Domains.Models.Lists.CreditAgents;
using Domains.Models.Loans; 
using Mapster;

namespace Application.Services.Lists
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

        public async Task<Result<string>> ChangeCreditAgents(int? branchId, List<LoanChangedCaDtoCreate> req)
        {

            var model = req.Adapt<List<LoanChangedCa>>();

            return await crService.ChangedCreditAgents(branchId, model);
        }


        public async Task<Result<IEnumerable<VCreditAgent>>> GetCreditAgents(bool includeInactive = false, string? query = null)
        {
            return await crService.GetCreditAgents(includeInactive, query);
        }

        public async Task<Pager<VCreditAgent>> GetCreditAgents(int? branchId, QueryParam param)
        {
            return await crService.GetCreditAgents(branchId, param);
        }

        public async Task<Result<IEnumerable<VEmployeeCreditAgent>>> GetEmployeeCreateCreditAgents(string? query = null)
        {
            return await crService.GetEmployeeCreateCreditAgents(query);
        }

        public async Task<Pager<VEmployeeCreditAgent>> GetEmployeeCreditAgents(int? branchId, string? employeeId, QueryParam param)
        {
            return await crService.GetEmployeeCreditAgents(branchId, employeeId, param);
        }

        public Task<Result<string>> UpdateCreditAgent(string id, LoanCreditAgentDtoUpdate req)
        {
            throw new NotImplementedException();
        }
    }
}
