using Application.Features.Transactions.LoanTransactions;
using ErrorOr;

namespace Application.Features.LoanPayments.Command.RecievePayment;

public record RecievePaymentCommand(LoanPaymentTransactionDtoCreate Request) : IRequest<ErrorOr<string>>;
