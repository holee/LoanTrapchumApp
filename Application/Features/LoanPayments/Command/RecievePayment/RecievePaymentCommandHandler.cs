using Application.Commons.Mappers;
using ErrorOr;

namespace Application.Features.LoanPayments.Command.RecievePayment;

public sealed class RecievePaymentCommandHandler(ILoanPaymentService _loanPaymentService) : IRequestHandler<RecievePaymentCommand, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(RecievePaymentCommand request, CancellationToken cancellationToken)
    {

        var model = request.RecieveLoanTransction();

        return await _loanPaymentService.RecievePayment(model, cancellationToken);

    }
}