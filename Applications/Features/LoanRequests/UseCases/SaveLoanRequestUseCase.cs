using Applications.Commons.Mappers.LoanRequests;
using Applications.Enumerations;
using Applications.Features.LoanRequests.Interfaces;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;

namespace Applications.Features.LoanRequests.UseCases
{
    public sealed class SaveLoanRequestUseCase : ISaveLoanRequestUseCase
    {
        private readonly ILoanRequestService _loanRequestService;

        public SaveLoanRequestUseCase(ILoanRequestService loanRequestService)
        {
            _loanRequestService = loanRequestService;
        }

        //public async Task<Result<RequestResult>> ExecuteAsync(LoanRequestDtoCreate request, CancellationToken cancellation = default)
        //{
        //    var req = request.MapToLoanRequestSave();

        //    req.LoanRequestStatusRefId = (int)RequestStatus.New;

        //    var rsp = await _loanRequestService.SaveLoanRequestAsync(req, cancellation);

        //    return rsp.IsSuccess ? Result.Success(new RequestResult(true, rsp.Data))
        //                         : Result.Failure<RequestResult>(ErrorResponse.ServerError(rsp.ErrMessage));
        //}

    }
}
