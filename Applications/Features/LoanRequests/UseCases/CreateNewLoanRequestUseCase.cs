using Applications.Features.LoanRequests.Interfaces;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;

namespace Applications.Features.LoanRequests.UseCases
{
    public sealed class CreateNewLoanRequestUseCase : ICreateNewLoanRequestUseCase
    {
        private readonly ILoanRequestService _loanRequestService;

        public CreateNewLoanRequestUseCase(ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService;
        }


        public async Task<Result<RequestResult>> ExecuteAsync(LoanRequestDtoCreate request, CancellationToken cancellation = default)
        {

            var req = request.MapToLoanCreateNew();

            var rsp = await _loanRequestService.CreateNewLoanRequestAsync(req, cancellation);

            return rsp.IsSuccess
                ? Result.Success<RequestResult>(new(true, rsp.Data))
                : Result.Failure<RequestResult>(ErrorResponse.ServerError(rsp.ErrMessage));
        }
    }
}
