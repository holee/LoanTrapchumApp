
using Applications.Features.LoanPayments.Interfaces;
using Applications.Interfaces.Loans;
using Applications.Responses;
using ErrorOr;

namespace Applications.Features.LoanPayments.UseCases
{
    public sealed class AddRecieptNumberUseCase : IAddRecieptNumber
    {
        private readonly ILoanPaymentService _loanPaymentService;

        public AddRecieptNumberUseCase(ILoanPaymentService loanPaymentService)
        {
            _loanPaymentService = loanPaymentService;
        }


        public async Task<ErrorOr<string?>> ExecuteAsync(CancellationToken cancellation = default)
        {
            var rsp = await _loanPaymentService.GenerateRecieptNumber(cancellation);

            if (!rsp.IsError)
            {

                var reciept = rsp.Value.RecieptNumber;

                return reciept;
            }

            return string.Empty;
        }
    }
}