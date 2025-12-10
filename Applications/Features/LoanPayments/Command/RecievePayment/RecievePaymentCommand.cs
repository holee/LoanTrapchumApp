using Applications.Features.Transactions.LoanTransactions;
using ErrorOr;

namespace Applications.Features.LoanPayments.Command.RecievePayment;

public record RecievePaymentCommand(LoanPaymentTransactionDtoCreate Request) : IRequest<ErrorOr<string>>;
