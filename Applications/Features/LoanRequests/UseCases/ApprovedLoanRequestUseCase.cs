using Applications.Features.LoanRequests.Interfaces;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;

namespace Applications.Features.LoanRequests.UseCases
{
    public sealed class ApprovedLoanRequestUseCase : IApprovedLoanRequestUseCase
    {
        private readonly ILoanRequestService _loanRequestService;

        public ApprovedLoanRequestUseCase(ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService; 
        }

        public async Task<Result<RequestResult>> ExecuteAsync(AproveLoanDtoCreate req) 
        {
            var rsp = await _loanRequestService.AprovedLoanRequestAsync(req);

            if (rsp.IsSuccess)
            {
                return Result.Success(new RequestResult(true, rsp.Data));
            }

            return Result.Failure<RequestResult>(ErrorResponse.Validation(rsp.ErrMessage));
        }


    }
}
