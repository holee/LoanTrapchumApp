using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Currencies.Command.AddNewCurrency;

public record AddNewCurrencyCommand(CurrencyDtoCreate Currency) : IRequest<ErrorOr<VCurrenciesList>>;
