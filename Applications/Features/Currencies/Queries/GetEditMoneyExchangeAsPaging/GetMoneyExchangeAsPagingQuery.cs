using Applications.Features.Currencies.Responses;

namespace Applications.Features.Currencies.Queries.GetEditMoneyExchange;

public record GetMoneyExchangeAsPagingQuery(int?  branchId,DateTimeOffset? TransactionDate,QueryParam Param) : IRequest<Pager<VttCurrencyExchanges>>; 