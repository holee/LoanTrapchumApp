using Application.Features.Currencies.Responses;

namespace Application.Features.Currencies.Queries.GetEditMoneyExchange;

public record GetMoneyExchangeAsPagingQuery(int?  branchId,DateTimeOffset? TransactionDate,QueryParam Param) : IRequest<Pager<VttCurrencyExchanges>>; 