using Applications.Features.Currencies.Requests;
using Applications.Features.Currencies.Responses;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Currencies.Queries.GetEditMoneyExchange;

public record GetMoneyExchangeAsPagingQuery(int?  branchId,DateTimeOffset? TransactionDate,QueryParam Param) : IRequest<Pager<VttCurrencyExchanges>>; 