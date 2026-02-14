using Applications.Features.Currencies.Responses;
using ErrorOr;

namespace Applications.Features.Currencies.Queries.GetCurrencyCounter;

public record GetCurrencyCounterQuery(int? CurrencyId, int? BranchId) : IRequest<Pager<VCurrencyCounter>>;
