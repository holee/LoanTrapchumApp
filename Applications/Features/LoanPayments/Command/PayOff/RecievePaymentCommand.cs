using Applications.Features.LoanPayments.Dtos;
using Applications.Features.Transactions.LoanTransactions;
using ErrorOr;

namespace Applications.Features.LoanPayments.Command.PayOff;
 
public record PayOffCommand(LoanPaymentTransactionDtoCreate Request) : IRequest<ErrorOr<PayOffResult>>;
