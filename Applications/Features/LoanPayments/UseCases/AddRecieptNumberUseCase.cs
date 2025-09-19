
using Applications.Features.LoanPayments.Interfaces;
using Applications.Interfaces.Loans;
using Applications.Responses;

namespace Applications.Features.LoanPayments.UseCases
{
    public sealed class AddRecieptNumberUseCase : IAddRecieptNumberUseCase
    {
        private readonly ILoanPaymentService _loanPaymentService;

        public AddRecieptNumberUseCase(ILoanPaymentService loanPaymentService)
        {
            _loanPaymentService = loanPaymentService;
        }


        public async Task<Result<string>> ExecuteAsync(CancellationToken cancellation = default)
        {
            var rsp = await _loanPaymentService.GenerateRecieptNumber(cancellation);

            if (rsp.IsSuccess && rsp.Data is not null)
            {

                var reciept = rsp.Data.RecieptNumber;

                return Result.Success(reciept!);
            }

            return Result.Failure<string>(ErrorResponse.ServerError(rsp.ErrMessage));
        }
    }
}