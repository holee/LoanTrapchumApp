using Applications.Features.CreditAgents.Requests;
using Applications.Interfaces.Lists;
using Domains.Models.Lists.CreditAgents;
using Mapster;

namespace Applications.Features.Loans.Services
{
    public sealed class LoanService(ILoanService loanService, ICreditAgentService creditAgentService) : ILoanServiceImpl
    {
        public async Task<Result<string>> ChangedCreditAgent(int? branchId, List<LoanChangedCaDtoCreate> req)
        {
            if (!branchId.HasValue)
            {
                return Result.Failure<string>(ErrorResponse.Validation("BranchId and Credit Agent details are required."));
            }

            var model = req.Adapt<List<LoanChangedCa>>();

            return await creditAgentService.ChangedCreditAgents(branchId, model);
        }
        
        public async Task<Result<VLoanDisbursments>> GetLoanByCreditAgentIdAsync(string? creditAgentId, string? loanId, int? branchId, CancellationToken cancellation = default)
        {
            if (string.IsNullOrEmpty(creditAgentId) || string.IsNullOrEmpty(loanId) || !branchId.HasValue)
            {
                return Result.Failure<VLoanDisbursments>(ErrorResponse.Validation($"{creditAgentId} OR {loanId} OR {branchId} is required,"));
            }


            var item = await loanService.GetLoanByCreditAgentIdAsync(creditAgentId, loanId, branchId, cancellation);

            if (item == null)
            {
                return Result.Failure<VLoanDisbursments>(ErrorResponse.NotFound("Loan Not Found."));
            }

            return Result.Success(item);

        }

        public async Task<Pager<VLoanDisbursments>> GetLoanByCreditAgentsAsync(string? creditAgentId, int? branchId, CancellationToken cancellation = default)
        {
            if (!branchId.HasValue)
                throw new ArgumentNullException(nameof(branchId), "Branch ID must not be null.");

            return await loanService.GetLoanByCreditAgentAsync(creditAgentId, branchId, cancellation);
        }

       
    }

}
