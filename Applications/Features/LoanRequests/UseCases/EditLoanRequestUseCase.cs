using Applications.Enumerations;
using Applications.Features.LoanRequests.Requests;
using Applications.Features.Loans.Responses;
using Applications.Interfaces.Loans;
using Applications.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.LoanRequests.UseCases
{
    public sealed class EditLoanRequestUseCase : IEditLoanRequestUseCase
    {
        private readonly ILoanRequestService _loanRequestService;

        public EditLoanRequestUseCase(ILoanRequestService loanRequestService) 
        {
            _loanRequestService = loanRequestService;
        }

        public async Task<Result<RequestResult>> ExecuteAsync(LoanRequestDtoCreate request, CancellationToken cancellation = default)
        {
            var req = request.MapToLoanRequestCreate();

            req.LoanRequestStatusRefId = (int)RequestStatus.New;

            var rsp = await _loanRequestService.EditLoanRequestAsync(req, cancellation);

            return rsp.IsSuccess ? Result.Success(new RequestResult(true, rsp.Data))
                                 : Result.Failure<RequestResult>(ErrorResponse.ServerError(rsp.ErrMessage));
        }
    }
}
