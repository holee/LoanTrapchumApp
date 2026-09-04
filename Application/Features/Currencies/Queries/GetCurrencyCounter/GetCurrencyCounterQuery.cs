using Application.Features.Currencies.Responses;
using ErrorOr;

namespace Application.Features.Currencies.Queries.GetCurrencyCounter;

public record GetCurrencyCounterQuery(int? CurrencyId, int? BranchId) : IRequest<Pager<VCurrencyCounter>>;
