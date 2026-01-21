using Applications.Features.Currencies.Requests;
using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public record EditMoneyExchangeCommand(ForeignExchangeDtoUpdate Request) : IRequest<ErrorOr<VttCurrencyExchanges>>;
