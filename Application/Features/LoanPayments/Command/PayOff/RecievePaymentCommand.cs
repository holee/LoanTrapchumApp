using Application.Features.LoanPayments.Dtos;
using Application.Features.Transactions.LoanTransactions;
using ErrorOr;

namespace Application.Features.LoanPayments.Command.PayOff;
 
public record PayOffCommand(LoanPaymentTransactionDtoCreate Request) : IRequest<ErrorOr<PayOffResult>>;
