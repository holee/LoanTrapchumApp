using Applications.Features.Currencies.Requests;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Currencies.Command.EditMoneyExchange;

public record EditMoneyExchangeCommand(ExchangeDtoEdit Request) : IRequest<ErrorOr<string>>;
