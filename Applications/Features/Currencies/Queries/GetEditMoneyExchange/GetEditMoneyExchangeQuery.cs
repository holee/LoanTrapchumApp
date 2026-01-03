using Applications.Features.Currencies.Requests;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.Currencies.Queries.GetEditMoneyExchange;

public record GetEditMoneyExchangeQuery(int? branchId, string? TransactionId, DateTimeOffset TransactionDate) : IRequest<ErrorOr<ExchangeDtoEdit>>; 