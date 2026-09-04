using Application.Enumerations;
using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces.Loans;
using Application.Responses.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Loans.UseCases
{
    public sealed class GetLoanByBranchUseCase
    {
        private readonly ILoanService _loanService;
        public GetLoanByBranchUseCase(ILoanService loanService)
        {
            _loanService = loanService;
        }
        public async Task<(Pager<VLoanDisbursments>,int counts)> ExecuteAsync(int? branchId, string? creditAgentId, LoanStatuses status = LoanStatuses.Active, QueryParam? param = null, CancellationToken cancellation = default)
        {
            return await _loanService.GetLoansByBranchAsync(branchId, creditAgentId, status,param,cancellation);
        }
    }
}
