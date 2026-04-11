using Applications.Commons.Mappers;
using Applications.Features.LoanPayments.Dtos;
using ErrorOr;

namespace Applications.Features.LoanPayments.Command.PayOff;

public sealed class PayOffCommandHandler(ILoanPaymentService _loanPaymentService) : IRequestHandler<PayOffCommand, ErrorOr<PayOffResult>>
{
    public async Task<ErrorOr<PayOffResult>> Handle(PayOffCommand request, CancellationToken cancellationToken)
    {

        var model = request.Request.ToLoanTransction();

        return await _loanPaymentService.PayOff(model, cancellationToken);

    }
}