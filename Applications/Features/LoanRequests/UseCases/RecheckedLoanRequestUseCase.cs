using Applications.Features.LoanRequests.Interfaces;
using Applications.Features.LoanRequests.Requests;
using Applications.Interfaces.Loans;
using Applications.Responses;
using Applications.Responses.ViewModels.Loans.Requests;

namespace Applications.Features.LoanRequests.UseCases
{
    public sealed class RecheckedLoanRequestUseCase : IRecheckedLoanRequestUseCase 
    {
        private readonly ILoanRequestService _loanRequestService;

        public RecheckedLoanRequestUseCase(ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService;
        }

        public async Task<Result<VLoanRequests>> ExecuteAsync(RecheckedRequestDtoCreate req)
        {
            return await _loanRequestService.RecheckedLoanRequestAsync(req);
        }

    }
}
