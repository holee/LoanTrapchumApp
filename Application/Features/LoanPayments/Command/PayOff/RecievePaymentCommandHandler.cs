using Application.Commons.Mappers;
using Application.Features.LoanPayments.Dtos;
using ErrorOr;

namespace Application.Features.LoanPayments.Command.PayOff;

public sealed class PayOffCommandHandler(ILoanPaymentService _loanPaymentService) : IRequestHandler<PayOffCommand, ErrorOr<PayOffResult>>
{
    public async Task<ErrorOr<PayOffResult>> Handle(PayOffCommand request, CancellationToken cancellationToken)
    {

        var model = request.Request.ToLoanTransction();

        return await _loanPaymentService.PayOff(model, cancellationToken);

    }
}