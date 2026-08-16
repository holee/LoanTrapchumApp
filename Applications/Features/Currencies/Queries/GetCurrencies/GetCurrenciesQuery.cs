using Domains.Models.Currencies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Currencies.Queries.GetCurrencies;

public record GetCurrenciesQuery(int? currencyId, QueryParam Param, params string[] Columns) : IRequest<PageMetaData<CurrencyDto>>;
