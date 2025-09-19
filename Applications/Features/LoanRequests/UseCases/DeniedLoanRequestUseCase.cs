using Applications.Features.LoanRequests.Interfaces;
using Applications.Features.LoanRequests.Requests;
using Applications.Interfaces.Loans;
using Applications.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.LoanRequests.UseCases
{
    public sealed class DeniedLoanRequestUseCase : IDeniedLoanRequestUseCase
    {
        private readonly ILoanRequestService _loanRequestService;

        public DeniedLoanRequestUseCase(ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService;
        }

        public async Task<Result<string>> ExecuteAsync(DeniedRequestDtoCreate req) 
        {
            return await _loanRequestService.DeniedLoanRequestAsync(req);
        }

    }
}
