using Applications.Features.Currencies.Requests;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetEditMoneyExchange;

public record GetEditMoneyExchangeQuery(int? branchId, string? TransactionId, DateTimeOffset TransactionDate) : IRequest<ErrorOr<ExchangeDtoEdit>>; 