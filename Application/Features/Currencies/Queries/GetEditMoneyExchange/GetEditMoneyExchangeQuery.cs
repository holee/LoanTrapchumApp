using Application.Features.Currencies.Dto;
using ErrorOr;

namespace Application.Features.Currencies.Queries.GetEditMoneyExchange;

public record GetEditMoneyExchangeQuery(int? branchId, string? TransactionId, DateTimeOffset TransactionDate) : IRequest<ErrorOr<ExchangeDtoEdit>>; 