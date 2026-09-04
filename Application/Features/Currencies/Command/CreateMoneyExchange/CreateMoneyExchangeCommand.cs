using Application.Features.Transactions.ForiegnExchanges;
using ErrorOr;

namespace Application.Features.Currencies.Command;

public record CreateMoneyExchangeCommand(ForeignExchangeDtoCreate Request) : IRequest<ErrorOr<VttCurrencyExchanges>>; 
